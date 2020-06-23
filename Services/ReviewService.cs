using BookStoreProject.Dtos.Review;
using BookStoreProject.Helpers;
using BookStoreProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        Task<bool> isPurchased(Review review);
        IEnumerable<ReviewForUserListDto> GetReviewsByCriteria(IEnumerable<ReviewForUserListDto> list, bool isLatest, bool? isPurchased);
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
                var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.BookID == reviewCreate.BookID);
                if (book == null)
                    return false;
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
                        .Where(delegate (Review r)
                        {
                            if (MyConvert.ConvertToUnSign(r.Book.NameBook.ToUpper()).IndexOf(keyword.ToUpper(), StringComparison.CurrentCultureIgnoreCase) >= 0 ||

                            MyConvert.ConvertToUnSign(r.ApplicationUser.Email.ToUpper()).IndexOf(keyword.ToUpper(), StringComparison.CurrentCultureIgnoreCase) >= 0 ||

                            MyConvert.ConvertToUnSign(r.Comment.ToUpper()).IndexOf(keyword.ToUpper(), StringComparison.CurrentCultureIgnoreCase) >= 0 ||

                            r.Book.NameBook.ToUpper().Contains(keyword.ToUpper()) ||

                            r.ApplicationUser.Email.ToUpper().Contains(keyword.ToUpper()) ||

                            r.Comment.Contains(keyword))

                                return true;
                            else
                                return false;
                        }
                        ).AsEnumerable();
            }
            return _dbContext.Reviews.Include(x => x.Book).Include(x => x.ApplicationUser).AsEnumerable();
        }

        public async Task<IEnumerable<Review>> GetReviewsByBookId(int bookId)
        {
            return await _dbContext.Reviews.Include(x => x.ApplicationUser).Where(x => x.BookID == bookId).ToListAsync();
        }

        public IEnumerable<ReviewForUserListDto> GetReviewsByCriteria(IEnumerable<ReviewForUserListDto> list, bool isLatest, bool? isPurchased)
        {
            return isLatest ? list.Where(x => isPurchased == null ? x.isPurchased != null : x.isPurchased == isPurchased)
                                    .OrderByDescending(x => x.Date.Year)
                                   .ThenByDescending(x => x.Date.Month)
                                   .ThenByDescending(x => x.Date.Day)
                                : list.Where(x => isPurchased == null ? x.isPurchased != null : x.isPurchased == isPurchased)
                                    .OrderBy(x => x.Date.Year)
                                   .ThenBy(x => x.Date.Month)
                                   .ThenBy(x => x.Date.Day);
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
