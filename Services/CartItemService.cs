using BookStoreProject.Dtos.CartItem;
using BookStoreProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Services
{
    public interface ICartItemService
    {
        Task<bool> CreateCartItem(CartItems cartItemCreate);
        Task<bool> UpdateCartItem(CartItems cartItemUpdate);
        Task<bool> DeleteCartItem(IEnumerable<CartItemForDeleteDto> bookIds, string applicationuserId);
        Task<IEnumerable<CartItems>> GetCartItemsByUserId(string applicationuserId);
    }
    public class CartItemService : ICartItemService
    {
        private readonly BookStoreDbContext _dbContext;
        public CartItemService (BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateCartItem(CartItems cartItemCreate)
        {
            try
            {
                _dbContext.CartItems.Add(cartItemCreate);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteCartItem(IEnumerable<CartItemForDeleteDto> bookIds, string applicationuserId)
        {
            try
            {
                if (bookIds != null)
                {
                    foreach (var item in bookIds)
                    {
                        var cartItemInDB = await _dbContext.CartItems
                                    .FirstOrDefaultAsync(x => x.BookID == item.BookId && x.ApplicationUserId == applicationuserId);
                        if (cartItemInDB == null)
                           continue;
                        _dbContext.Remove(cartItemInDB);
                    }    
                }    
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<CartItems>> GetCartItemsByUserId(string applicationuserId)
        {
            return await _dbContext.CartItems.Include(x => x.Book).Where(x => x.ApplicationUserId == applicationuserId).ToListAsync();
        }

        public async Task<bool> UpdateCartItem(CartItems cartItemUpdate)
        {
            try
            {
                _dbContext.CartItems.Update(cartItemUpdate);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
