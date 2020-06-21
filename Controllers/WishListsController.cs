using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreProject.Models;
using BookStoreProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class WishListsController : ControllerBase
    {
        private readonly IWishListService _wishListService;
        public WishListsController (IWishListService wishListService)
        {
            _wishListService = wishListService;
        }
        [HttpPost("{bookId}")]
        public async Task<IActionResult> CreateWishItem(int bookId)
        {
            var userId = GetUserId();
            if (userId == "error")
            {
                return Unauthorized();
            }
            var wishItem = new WishList()
            {
                ApplicationUserId = userId,
                BookID = bookId
            };
            var result = await _wishListService.CreateWishItem(wishItem);
            if (result)
                return Ok();
            return BadRequest();
        }
        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetWishListItem(int bookId)
        {
            var userId = GetUserId();
            if (userId == "error")
            {
                return Unauthorized();
            }
            var wishItem = await _wishListService.GetWishItem(bookId, userId);
            if (wishItem == null)
                return Ok(new { isLike = false });
            return Ok(new { isLike = true });
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
