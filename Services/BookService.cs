using AutoMapper;
using BookStoreProject.Dtos.Book;
using BookStoreProject.Helpers;
using BookStoreProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookStoreProject.Services
{
    public interface IBookService
    {
        Task<bool> DeleteBookAsync(int bookId);
        Task<Book> GetBookByIdAsync(int bookId);
        Task<Book> GetBookByIdForUserAsync(int bookId);
        Task<bool> CreateBookAsync(Book bookCreate);
        Task<bool> UpdateBookAsync(Book book);
        IEnumerable<Book> GetBooks(string keyword);
        IEnumerable<BookForListDto> GetBooksPerPage(IEnumerable<BookForListDto> list, int page = 1, int pageSize = 10, int sort = 0, string criteria = "BookId");
        Task<IEnumerable<BookForUserListDto>> GetLatestBooks(int? num);
        Task<IEnumerable<BookForUserPopularListDto>> GetPopularBooks(int? num);
        Task<ICollection<Book>> GetRelatedBooks(int bookId, int num);
        IEnumerable<Book> GetBooksForUser(string keyword);
        Task<IEnumerable<Book>> GetBooksByCategoryId(int categoryId);

        Task<bool> isPurchased(Review review);
    }
    public class BookService : IBookService
    {
        private readonly BookStoreDbContext _dbContext;
        private readonly IMapper _mapper;
        public BookService(BookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            return await _dbContext.Books.FirstOrDefaultAsync(x => x.BookID == bookId);
        }
        public async Task<bool> DeleteBookAsync(int bookID)
        {
            try
            {
                var book = await _dbContext.Books.Include(x => x.WishLists)
                            .Include(x => x.OrderItems)
                            .Include(x => x.Reviews)
                            .Include(x => x.CartItems)
                            .FirstOrDefaultAsync(x => x.BookID == bookID);
                if (book == null)
                    return false;

                if (book.CartItems.Any())
                {
                    _dbContext.CartItems.RemoveRange(book.CartItems);
                }

                if (book.OrderItems.Any())
                {
                    _dbContext.OrderItems.RemoveRange(book.OrderItems);
                }

                if (book.WishLists.Any())
                {
                    _dbContext.WishLists.RemoveRange(book.WishLists);
                }

                if (book.Reviews.Any())
                {
                    _dbContext.Reviews.RemoveRange(book.Reviews);
                }
                _dbContext.Books.Remove(book);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
        public async Task<bool> CreateBookAsync(Book bookCreate)
        {
            try
            {
                
                _dbContext.Books.Add(bookCreate);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> UpdateBookAsync(Book book)
        {
            try
            {

                _dbContext.Books.Update(book);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<Book> GetBooks(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {

                return _dbContext.Books
                    .Where(delegate (Book b)
                    {
                        if (MyConvert.ConvertToUnSign(b.NameBook.ToUpper()).IndexOf(keyword.ToUpper(), StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                        MyConvert.ConvertToUnSign(b.Author.ToUpper()).IndexOf(keyword.ToUpper(), StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                        b.NameBook.ToUpper().Contains(keyword.ToUpper()) ||
                        b.Author.ToUpper().Contains(keyword.ToUpper()))
                            return true;
                        else
                            return false;
                    })
                    .AsEnumerable();
            }
            return _dbContext.Books.AsEnumerable();
        }
        public IEnumerable<BookForListDto> GetBooksPerPage(IEnumerable<BookForListDto> list, int page = 1, int pageSize = 10, int sort = 0, string criteria = "BookId")
        {
            criteria = criteria.ToLower();
            
            if (criteria.Equals("bookid"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.BookID).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.BookID).Skip((page - 1) * pageSize).Take(pageSize);
            }
            if (criteria.Equals("namebook"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.NameBook).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.NameBook).Skip((page - 1) * pageSize).Take(pageSize);
            }
            if (criteria.Equals("author"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.Author).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.Author).Skip((page - 1) * pageSize).Take(pageSize);
            }
            
            if (criteria.Equals("status"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.Status).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.Status).Skip((page - 1) * pageSize).Take(pageSize);
            }
            if (criteria.Equals("quantityin"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.QuantityIn).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.QuantityIn).Skip((page - 1) * pageSize).Take(pageSize);
            }
            if (criteria.Equals("quantityout"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.QuantityOut).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.QuantityOut).Skip((page - 1) * pageSize).Take(pageSize);
            }
            if (criteria.Equals("price"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.Price).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.Price).Skip((page - 1) * pageSize).Take(pageSize);
            }
            return null;
        }

        public async Task<IEnumerable<BookForUserListDto>> GetLatestBooks(int? num)
        {
            var latestBookList = await _dbContext.Books.Include(x => x.Reviews).Where(x => x.Status == true)
                                    .OrderByDescending(x => x.Date.Year)
                                    .ThenByDescending(x => x.Date.Month)
                                    .ThenByDescending(x => x.Date.Day).ToListAsync();
            var listForReturn = latestBookList.Select(x => new BookForUserListDto
            {
                BookID = x.BookID,
                NameBook = x.NameBook,
                OriginalPrice = x.OriginalPrice,
                Price = x.Price,
                Information = x.Information,
                ReviewCount = x.Reviews.Any() ? x.Reviews.Count : 0,
                ImageLink = x.ImageLink,
                Rating = x.Reviews.Any() ?  (int) Math.Round((double)(x.Reviews.Aggregate((a,b) => new Review { Rating = a.Rating + b.Rating}).Rating / (double)x.Reviews.Count)) : 0
            }) ;
            return num != null ? listForReturn.Take((int)num) : listForReturn; 
        }

        public async Task<IEnumerable<BookForUserPopularListDto>> GetPopularBooks(int? num)
        {
            /* var latestBookList = await _dbContext.Books.Include(x => x.OrderItems)
                                     .ThenInclude(x => x.Order).ToListAsync();
                                     */
            /* var latestBookList = await _dbContext.Books.Include(x => x.Reviews).Where(x => x.Status == true).OrderByDescending(x => x.QuantityOut).Take(number).ToListAsync();
             var listForReturn = latestBookList.Select(x => new BookForUserListDto
             {
                 BookID = x.BookID,
                 NameBook = x.NameBook,
                 Price = x.Price,
                 ReviewCount = x.Reviews.Count,
                 ImageLink = x.ImageLink,
                 Rating = (int)(x.Reviews.Aggregate((a, b) => new Review { Rating = a.Rating + b.Rating }).Rating / x.Reviews.Count)
             });*/

         var list = (await _dbContext.OrderItems.Include(x => x.Order)
                                       .Include(x => x.Book).ThenInclude(y => y.Reviews)
                                       .Where(x => x.Order.Date.Year == DateTime.Now.Year && x.Order.Date.Month == DateTime.Now.Month)
                                       .ToListAsync())
                                        .GroupBy(x => x.Book)
                                        .Where(x => x.Key.Status == true)
                                      .OrderByDescending(x => x.Aggregate((a,b) => new OrderItems {Quantity = a.Quantity + b.Quantity }).Quantity)
                                        .Select(x => new BookForUserPopularListDto
                                        {
                                            BookID = x.Key.BookID,
                                            NameBook = x.Key.NameBook,
                                            OriginalPrice = x.Key.OriginalPrice,
                                            Price = x.Key.Price,
                                            Information = x.Key.Information,
                                            Author = x.Key.Author,
                                            ReviewCount = x.Key.Reviews.Any() ? x.Key.Reviews.Count : 0,
                                            ImageLink = x.Key.ImageLink,
                                            Rating = x.Key.Reviews.Any() ? (int)Math.Round( (double) (x.Key.Reviews.Aggregate((a, b) => new Review { Rating = a.Rating + b.Rating }).Rating / (double) x.Key.Reviews.Count)) : 0
                                        }).ToList();
            return num != null ? list.Take((int)num) : list;
            //return listForReturn;
        }

        public async Task<ICollection<Book>> GetRelatedBooks(int bookId, int number)
        {
            Random rnd = new Random();
            var book = await GetBookByIdAsync(bookId);
            var relatedBooks = await _dbContext.Books.Where(x => x.CategoryID == book.CategoryID 
                                                    || x.PublisherID == book.PublisherID
                                                    && x.Status == true
                                                    && x.BookID != book.BookID)
                .ToListAsync();
            relatedBooks =  relatedBooks.Skip(rnd.Next(0,relatedBooks.Count() / 2)).Take(number).ToList();
            return relatedBooks;
        }

        public async Task<Book> GetBookByIdForUserAsync(int bookId)
        {
            return await _dbContext.Books.Include(x => x.Publisher)
                          .Include(x => x.Category)
                          .Include(x => x.Reviews).ThenInclude(y => y.ApplicationUser)
                          .FirstOrDefaultAsync(x => x.BookID == bookId && x.Status == true);
        }

        public IEnumerable<Book> GetBooksForUser(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {

                return _dbContext.Books.Include(x => x.Reviews)
                    .Where(delegate (Book b)
                    {
                        if (MyConvert.ConvertToUnSign(b.NameBook.ToUpper()).IndexOf(keyword.ToUpper(), StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                        MyConvert.ConvertToUnSign(b.Author.ToUpper()).IndexOf(keyword.ToUpper(), StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                        b.NameBook.ToUpper().Contains(keyword.ToUpper()) ||
                        b.Author.ToUpper().Contains(keyword.ToUpper()))
                            return true;
                        else
                            return false;
                    })
                    .AsEnumerable();
            }
            return _dbContext.Books.Include(x => x.Reviews).AsEnumerable();
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryId(int categoryId)
        {
            return await _dbContext.Books.Where(x => x.CategoryID == categoryId).ToListAsync();
        }
        public async Task<bool> isPurchased(Review review)
        {
            var purchasedBook = await _dbContext.OrderItems.Include(x => x.Order)
                                 .FirstOrDefaultAsync(x => x.BookID == review.BookID && x.Order.ApplicationUserID == review.ApplicationUserId);
            if (purchasedBook == null)
                return false;
            return true;
        }
        
    }
}
