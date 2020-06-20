using BookStoreProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Services
{
    public interface IPublisherService
    {
        Task<bool> CreatePublisherAsync(Publisher publisherCreate);
        Task<bool> UpdatePublisherAsync(Publisher publisherUpdate);
        Task<bool> DeletePublisherAsync(int publisherId);
        IEnumerable<Publisher> GetPublishers(string keyword);
        Task<IEnumerable<Publisher>> GetPublishers();
        Task<Publisher> GetPublisherByIdAsync(int publisherId);
        int CountBookTitleInPublisher(int publisherId);
    }
    public class PublisherService : IPublisherService
    {
        private readonly BookStoreDbContext _dbContext;
        public PublisherService(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int CountBookTitleInPublisher(int publisherId)
        {
            var publisher = _dbContext.Publishers.Include(x => x.Books).FirstOrDefault(x => x.PublisherID == publisherId);
            return publisher.Books.Count;
        }

        public async Task<bool> CreatePublisherAsync(Publisher publisherCreate)
        {
            try
            {
                _dbContext.Publishers.Add(publisherCreate);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeletePublisherAsync(int publisherId)
        {
            try
            {
                var publisherInDb = await _dbContext.Publishers.Include(x => x.Books).ThenInclude(y => y.CartItems)
                                .Include(x => x.Books).ThenInclude(y => y.Reviews)
                                .Include(x => x.Books).ThenInclude(y => y.WishLists)
                                .Include(x => x.Books).ThenInclude(y => y.OrderItems)
                                .FirstOrDefaultAsync(x => x.PublisherID == publisherId);
                if (publisherInDb == null)
                    return false;
                if (publisherInDb.Books.Any())
                {
                    foreach (var book in publisherInDb.Books)
                    {

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
                    }
                    _dbContext.Books.RemoveRange(publisherInDb.Books);
                }    
                _dbContext.Publishers.Remove(publisherInDb);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Publisher> GetPublisherByIdAsync(int publisherId)
        {
            return await _dbContext.Publishers.FirstOrDefaultAsync(x => x.PublisherID == publisherId);
        }

        public IEnumerable<Publisher> GetPublishers(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _dbContext.Publishers.Where(x =>
                        x.publisher.ToUpper().Contains(keyword.ToUpper()))
                    .AsEnumerable();
            }
            return _dbContext.Publishers.AsEnumerable();
        }

        public async Task<IEnumerable<Publisher>> GetPublishers()
        {
            return await _dbContext.Publishers.ToListAsync();
        }

        public async Task<bool> UpdatePublisherAsync(Publisher publisherUpdate)
        {
            try
            {
                _dbContext.Publishers.Update(publisherUpdate);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
