using BookStoreProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreProject.Services
{
    public interface IReviewService
    {
        Task<bool> DeleteReview(int bookId, string applicationUserId);
        IEnumerable<Review> GetReviews(string keyword);
    }
    public class ReviewService : IReviewService
    {
        private readonly BookStoreDbContext _dbContext;
        public ReviewService(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> DeleteReview(int bookId, string applicationUserId )
        {
            try
            {
                var review = await _dbContext.Reviews.Where(x => 
                x.BookID == bookId &&
                x.ApplicationUserId == applicationUserId)
                   .FirstOrDefaultAsync();
                if (review == null)
                    return false;
                _dbContext.Reviews.Remove(review);
                await _dbContext.SaveChangesAsync();
                return true;
            } 
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Review> GetReviews(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return  _dbContext.Reviews.Include(x => x.Book).Include(x => x.ApplicationUser)
                        .Where(x =>
                        x.Book.NameBook.ToUpper().Contains(keyword.ToUpper()) ||
                        x.ApplicationUser.UserName.ToUpper().Contains(keyword.ToUpper()) ||
                        x.Comment.Contains(keyword)).AsEnumerable();
            }
            return _dbContext.Reviews.Include(x => x.Book).Include(x => x.ApplicationUser).AsEnumerable();
        }
    }
}
