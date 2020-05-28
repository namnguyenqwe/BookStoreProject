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
        Task<bool> CreateBookAsync(Book bookCreate);
        Task<bool> UpdateBookAsync(Book book);
        IEnumerable<Book> GetBooks(string keyword);
        IEnumerable<BookForListDto> GetBooksPerPage(IEnumerable<BookForListDto> list, int page = 1, int pageSize = 10, int sort = 0, string criteria = "BookId");
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
    }
}
