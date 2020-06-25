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
using Microsoft.AspNetCore.Mvc.Formatters;

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
        [HttpPut("{bookId}")]
        public async Task<IActionResult> UpdateCartItem(int bookId, [FromBody] CartItemForUserUpdateDto input)
        {
            if (ModelState.IsValid)
            {
                var userId = GetUserId();
                if (userId == "error")
                {
                    return Unauthorized();
                }
                var cartItemInDB = await _cartItemService.GetCartItemById(bookId, userId);
                var result = await _cartItemService.UpdateCartItem(_mapper.Map(input, cartItemInDB));
                if (result)
                    return RedirectToAction("GetCartItemsByUserId");
            }
            return BadRequest(ModelState);
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
                return Ok(new { data = response });
            }
            catch (System.Exception)
            {
                return BadRequest();
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreateCartItem([FromBody] CartItemForUserCreateDto input)
        {
            try
            {
                var userId = GetUserId();
                if (userId == "error")
                {
                    return Unauthorized();
                }
                var cartItemInDB = await _cartItemService.GetCartItemById(input.BookId, userId);
                if (cartItemInDB != null)
                {
                    cartItemInDB.Quantity++;
                    var isSuccess = await _cartItemService.UpdateCartItem(cartItemInDB);
                    if (isSuccess)
                        return Ok();
                    return BadRequest(new { message = "Có lỗi trong quá trình lưu dữ liệu"});
                }    
                var cartItem = new CartItems()
                {
                    BookID = input.BookId,
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
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteCartItem(int bookId)
        {
            var userId = GetUserId();
            if (userId == "error")
            {
                return Unauthorized();
            }
            var cartItem = await _cartItemService.GetCartItemById(bookId, userId);
            if (cartItem == null)
            {
                return NotFound(bookId);
            }
            var result = await _cartItemService.DeleteCartItem(bookId, userId);
            if (!result)
            {
                return BadRequest("Có lỗi trong quá trình xóa dữ liệu: ");
            }
            return RedirectToAction("GetCartItemsByUserId");
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
