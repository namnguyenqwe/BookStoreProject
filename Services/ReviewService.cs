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
        Task<bool> DeleteReview(int reviewId);
        Task<bool> CreateReview(Review reviewCreate);
        IEnumerable<Review> GetReviews(string keyword);
        Task<IEnumerable<Review>> GetReviewsByBookId(int bookId);
        Task<Review> GetReviewById(int reviewId);
    }
    public class ReviewService : IReviewService
    {
        private readonly BookStoreDbContext _dbContext;
        public ReviewService(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateReview(Review reviewCreate)
        {
            try
            {
                _dbContext.Reviews.Add(reviewCreate);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteReview(int reviewId)
        {
            try
            {
                var review = await _dbContext.Reviews
                   .FirstOrDefaultAsync(x =>
                x.ReviewId == reviewId);
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

        public async Task<Review> GetReviewById(int reviewId)
        {
            return await _dbContext.Reviews.Include(x => x.ApplicationUser)
                            .Include(x => x.Book)
                .FirstOrDefaultAsync(x => x.ReviewId == reviewId);
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

        public async Task<IEnumerable<Review>> GetReviewsByBookId(int bookId)
        {
            return await _dbContext.Reviews.Include(x => x.ApplicationUser).Where(x => x.BookID == bookId).ToListAsync();
        }
    }
}
