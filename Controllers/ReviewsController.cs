using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreProject.Dtos.Review;
using BookStoreProject.Helpers;
using BookStoreProject.Models;
using BookStoreProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        public readonly IReviewService _reviewService;
        public readonly IMapper _mapper;
        public ReviewsController(IReviewService reviewService, IMapper mapper)
        {
            _reviewService = reviewService;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetReviews(string keyword, int page = 1, int pageSize = 10, int sort = 0, string criteria = "reviewid")
        {
            try
            {
                var reviewsInDB = _reviewService.GetReviews(keyword);
                var totalCount = reviewsInDB.Count();
                criteria = criteria.ToLower();
                var response = _mapper.Map<IEnumerable<Review>, IEnumerable<ReviewForListDto>>(reviewsInDB);
                if (criteria.Equals("reviewid"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.ReviewId).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.ReviewId).Skip((page - 1) * pageSize).Take(pageSize);
                }
                if (criteria.Equals("username"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.UserName).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.UserName).Skip((page - 1) * pageSize).Take(pageSize);
                }
                if (criteria.Equals("namebook"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.NameBook).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.NameBook).Skip((page - 1) * pageSize).Take(pageSize);
                }
                if (criteria.Equals("rating"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.Rating).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.Rating).Skip((page - 1) * pageSize).Take(pageSize);
                }
                if (criteria.Equals("comment"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.Comment).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.Comment).Skip((page - 1) * pageSize).Take(pageSize);
                }
                if (criteria.Equals("date"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.Date.Year)
                                                .ThenByDescending(x => x.Date.Month)
                                                .ThenByDescending(x => x.Date.Day).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.Date.Year)
                                        .ThenBy(x => x.Date.Month)
                                        .ThenBy(x => x.Date.Day).Skip((page - 1) * pageSize).Take(pageSize);
                }
                var paginationSet = new PaginationSet<ReviewForListDto>()
                {
                    Items = response,
                    Total = totalCount,
                };

                return Ok(paginationSet);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet("{reviewId}")]
        public async Task<IActionResult> GetReviewById(int reviewId)
        {
            var review = await _reviewService.GetReviewById(reviewId);
            if (review == null)
                return NotFound(reviewId);
            return Ok(_mapper.Map<ReviewForListDto>(review));
        }
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var review = await _reviewService.GetReviewById(reviewId);
            if (review == null)
                return null;
            var result = await _reviewService.DeleteReview(reviewId);
            if (!result)
                return BadRequest();
            return Ok();
        }
        [HttpGet("user/{bookId}")]
        public async Task<IActionResult> GetReviews(int bookId)
        {
            try
            {
                var reviews = await _reviewService.GetReviewsByBookId(bookId);
                var response = new ReviewForUserPageListDto()
                {
                    ReviewCount = reviews.Count(),
                    Rating = (float)(reviews.Aggregate((a, b) => new Review { Rating = a.Rating + b.Rating }).Rating / (float)reviews.Count()),
                    Reviews = _mapper.Map<IEnumerable<Review>, IEnumerable<ReviewForUserListDto>>(reviews)
                };
                return Ok(response);
            }
            catch(System.Exception)
            {
                return BadRequest();
            }
         }
        [Authorize(Roles = "User" )]
        [HttpPost]
        public async Task<IActionResult> CreateReview( [FromBody] ReviewForUserCreateDto input)
        {
            if (ModelState.IsValid)
            {
                var userId = GetUserId();
                if (userId == "error")
                {
                    return Unauthorized();
                }
                input.ApplicationUserId = userId;
                var review = _mapper.Map<Review>(input);
                var result = await _reviewService.CreateReview(review);
                if (result)
                    return Ok();
            }
            return BadRequest(ModelState);
        }
        [NonAction]
        public string GetUserId()
        {
            string userId;
            try
            {
                userId = User.Claims.First(c => c.Type == "UserID").Value;
            }
            catch
            {
                return "error";
            }
            return userId;
        }
    }
}
