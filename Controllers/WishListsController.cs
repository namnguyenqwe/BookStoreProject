﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreProject.Dtos.WishList;
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
        private readonly IMapper _mapper;
        public WishListsController (IWishListService wishListService, IMapper mapper)
        {
            _wishListService = wishListService;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> CreateWishItem([FromBody] WishListForCreateDto input)
        {
            var userId = GetUserId();
            if (userId == "error")
            {
                return StatusCode(201,new { message = "unauthorized"});
            }
            var wishItem = new WishList()
            {
                ApplicationUserId = userId,
                BookID = input.BookId
            };
            var result = await _wishListService.CreateWishItem(wishItem);
            if (result)
                return Ok();
            return StatusCode(201, new { message = "bad request" });
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
        [HttpGet]
        public async Task<IActionResult> GetWishList()
        {
            var userId = GetUserId();
            if (userId == "error")
            {
                return Unauthorized();
            }
            var wishList = await _wishListService.GetWishList(userId);
            var wishListForReturn = _mapper.Map<IEnumerable<WishList>, IEnumerable<WishListForUserListDto>>(wishList);
            if (wishList == null)
                return NotFound();
            return Ok(new { data = wishListForReturn });
        }
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteWishItem(int bookId)
        {
            var userId = GetUserId();
            if (userId == "error")
            {
                return Unauthorized();
            }
            var wishItemInDB = await _wishListService.GetWishItem(bookId, userId);
            if (wishItemInDB == null)
                return NotFound();
            var result = await _wishListService.DeleteWishList(bookId, userId);
            if (!result)
                return BadRequest(new { message = "Có lỗi trong quá trình xóa dữ liệu" });
            return Ok();
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
