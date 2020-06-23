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
    public interface ICategoryService
    {
        Task<bool> DeleteCategoryByIdAsync(int CategoryId);
        Task<bool> UpdateCategoryAsync(Categories categoryUpdate);
        Task<bool> CreateCategoryAsync(Categories categoryCreate);
        Task<Categories> FindCategoryByIdAsync(int CategoryId);
        IEnumerable<Categories> GetCategories(string keyword);
        Task<IEnumerable<Categories>> GetCategoriesAsync();
        int CountBookTitleInCategory(int CategoryId);
    }
    public class CategoryService : ICategoryService
    {
        private readonly BookStoreDbContext _dbContext;
        public CategoryService(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateCategoryAsync(Categories categoryCreate)
        {
            try
            {
                _dbContext.Categories.Add(categoryCreate);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> DeleteCategoryByIdAsync(int CategoryId)
        {
            try
            {
                var categoryInDb = await _dbContext.Categories.Include(x => x.Books).ThenInclude(y => y.CartItems)
                                .Include(x => x.Books).ThenInclude(y => y.Reviews)
                                .Include(x => x.Books).ThenInclude(y => y.WishLists)
                                .Include(x => x.Books).ThenInclude(y => y.OrderItems)
                                    .FirstOrDefaultAsync(x => x.CategoryID == CategoryId);
                if (categoryInDb == null)
                    return false;
                if (categoryInDb.Books.Any())
                {
                    foreach (var book in categoryInDb.Books)
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
                    _dbContext.Books.RemoveRange(categoryInDb.Books);
                }    
                _dbContext.Categories.Remove(categoryInDb);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateCategoryAsync(Categories categoryUpdate)
        {
            try
            {
                _dbContext.Categories.Update(categoryUpdate);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Categories> FindCategoryByIdAsync(int CategoryId)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(x => x.CategoryID == CategoryId);
        }
        public IEnumerable<Categories> GetCategories(string keyword)
        {
            if(!string.IsNullOrEmpty(keyword))
            {
                return _dbContext.Categories.Where(delegate (Categories c)
                {
                    if (MyConvert.ConvertToUnSign(c.Category.ToUpper()).IndexOf(keyword.ToUpper(), StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    c.Category.ToUpper().Contains(keyword.ToUpper()))
                        return true;
                    else
                        return false;
                })
                    .AsEnumerable();
            }
            return _dbContext.Categories.AsEnumerable();
        }

        public async Task<IEnumerable<Categories>> GetCategoriesAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public int CountBookTitleInCategory(int CategoryId)
        {
            var category = _dbContext.Categories.Include(x => x.Books)
                           .FirstOrDefault(x => x.CategoryID == CategoryId);
            return category.Books.Count;
        }

    }
}
