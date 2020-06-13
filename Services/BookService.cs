using AutoMapper;
using BookStoreProject.Dtos.Book;
using BookStoreProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        Task<IEnumerable<BookForUserListDto>> GetLatestBooks(int number);
        Task<IEnumerable<BookForUserListDto>> GetPopularBooks(int number);
        Task<ICollection<Book>> GetRelatedBooks(int bookId, int number);
        IEnumerable<Book> GetBooksForUser(string keyword);
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
                var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.BookID == bookID);
                if (book == null)
                    return false;
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

                return _dbContext.Books.Include(x => x.Category).Include(x => x.Publisher)
                    .Where(x =>
                        x.NameBook.ToUpper().Contains(keyword.ToUpper()) ||
                        x.Author.ToUpper().Contains(keyword.ToUpper()))
                    .AsEnumerable();
            }
            return _dbContext.Books.Include(x => x.Category).Include(x => x.Publisher).AsEnumerable();
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
            if (criteria.Equals("category"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.Category).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.Category).Skip((page - 1) * pageSize).Take(pageSize);
            }
            if (criteria.Equals("publisher"))
            {
                if (sort == 0)
                {
                    return list.OrderByDescending(x => x.publisher).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else
                    return list.OrderBy(x => x.publisher).Skip((page - 1) * pageSize).Take(pageSize);
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

        public async Task<IEnumerable<BookForUserListDto>> GetLatestBooks(int num)
        {
            var latestBookList = await _dbContext.Books.Include(x => x.Reviews).Where(x => x.Status == true)
                                    .OrderByDescending(x => x.Date.Year)
                                    .ThenByDescending(x => x.Date.Month)
                                    .ThenByDescending(x => x.Date.Day).Take(num).ToListAsync();
            var listForReturn = latestBookList.Select(x => new BookForUserListDto
            {
                BookID = x.BookID,
                NameBook = x.NameBook,
                Price = x.Price,
                ReviewCount = x.Reviews.Any() ? x.Reviews.Count : 0,
                ImageLink = x.ImageLink,
                Rating = x.Reviews.Any() ? (int)(x.Reviews.Aggregate((a,b) => new Review { Rating = a.Rating + b.Rating}).Rating / x.Reviews.Count) : 0
            }) ;
            return listForReturn; 
        }

        public async Task<IEnumerable<BookForUserListDto>> GetPopularBooks(int num)
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

            return await _dbContext.OrderItems.Include(x => x.Order)
                                       .Include(x => x.Book).ThenInclude(y => y.Reviews)
                                       // .Where(x => x.Order.Date.Month == DateTime.Now.Month)
                                        .GroupBy(x => x.Book)
                                        .OrderByDescending(x => x.Count())
                                        //.OrderByDescending(x => x.Aggregate((a,b) => new OrderItems {Quantity = a.Quantity + b.Quantity }).Quantity)
                                        .Select(x => new BookForUserListDto
                                        {
                                            BookID = x.Key.BookID,
                                            NameBook = x.Key.NameBook,
                                            Price = x.Key.Price,
                                            ReviewCount = x.Key.Reviews.Any() ? x.Key.Reviews.Count : 0,
                                            ImageLink = x.Key.ImageLink,
                                            Rating = x.Key.Reviews.Any() ? (int)(x.Key.Reviews.Aggregate((a, b) => new Review { Rating = a.Rating + b.Rating }).Rating / x.Key.Reviews.Count) : 0
                                        }).Take(num).ToListAsync();
                                        
            //return listForReturn;
        }

        public async Task<ICollection<Book>> GetRelatedBooks(int bookId, int number)
        {
            var book = await GetBookByIdAsync(bookId);
            return await _dbContext.Books.Where(x => x.CategoryID == book.CategoryID 
                                                    && x.Status == true
                                                    && x.BookID != book.BookID).Take(number).ToListAsync();
        }

        public async Task<Book> GetBookByIdForUserAsync(int bookId)
        {
            return await _dbContext.Books.Include(x => x.Publisher)
                          .Include(x => x.Category)
                          .Include(x => x.Reviews).ThenInclude(y => y.ApplicationUser)
                          .FirstOrDefaultAsync(x => x.BookID == bookId);
        }

        public IEnumerable<Book> GetBooksForUser(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {

                return _dbContext.Books.Include(x => x.Reviews)
                    .Where(x =>
                        x.NameBook.ToUpper().Contains(keyword.ToUpper()) ||
                        x.Author.ToUpper().Contains(keyword.ToUpper()))
                    .AsEnumerable();
            }
            return _dbContext.Books.Include(x => x.Reviews).AsEnumerable();
        }
    }
}
