using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreProject.Dtos.CartItem;
using BookStoreProject.Models;
using BookStoreProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;
        private readonly IMapper _mapper;
        public CartItemsController(ICartItemService cartItemService, IMapper mapper)
        {
            _cartItemService = cartItemService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetCartItemsByUserId()
        {
            try
            {
                var userId = GetUserId();
                if (userId == "error")
                {
                    return Unauthorized();
                }
                var cartItems = await _cartItemService.GetCartItemsByUserId(userId);
                var response = _mapper.Map<IEnumerable<CartItems>, IEnumerable<CartItemForUserListDto>>(cartItems);
                return Ok(response);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }

        }
        [HttpPost("{bookId}")]
        public async Task<IActionResult> CreateCartItem(int bookId)
        {
            try
            {
                var userId = GetUserId();
                if (userId == "error")
                {
                    return Unauthorized();
                }
                var cartItem = new CartItems()
                {
                    BookID = bookId,
                    ApplicationUserId = userId,
                    Quantity = 1,
                    CreatedDate = DateTime.Now
                };
                var result = await _cartItemService.CreateCartItem(cartItem);
                if (result)
                    return Ok();
                return BadRequest();
            }
            catch(System.Exception)
            {
                return BadRequest();
            }
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
