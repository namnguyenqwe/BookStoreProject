using BookStoreProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Services
{
    public interface IWishListService
    {
        Task<bool> CreateWishItem(WishList wishItemCreate);
        Task<WishList> GetWishItem(int bookId, string applicationUserId);
    }
    public class WishListService : IWishListService
    {
        private readonly BookStoreDbContext _dbContext;
        public WishListService (BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateWishItem(WishList wishItemCreate)
        {
            try
            {
                var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.BookID == wishItemCreate.BookID);
                if (book == null)
                    return false;
                _dbContext.WishLists.Add(wishItemCreate);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<WishList> GetWishItem(int bookId, string applicationUserId)
        {
            return await _dbContext.WishLists.FirstOrDefaultAsync(x => x.BookID == bookId && x.ApplicationUserId == applicationUserId);
        }
    }
}
