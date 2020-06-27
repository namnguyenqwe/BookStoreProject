using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BookStoreProject.Models;
using BookStoreProject.Services;
using BookStoreProject.Commons;
using BookStoreProject.Helpers;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using BookStoreProject.Dtos.Order;
using System.Collections.Generic;
using System;
using System.Globalization;

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IOrdersService _ordersService;
        private readonly IMapper _mapper;

        public OrdersController(IOrdersService ordersService, IMapper mapper)
        {
            _ordersService = ordersService;
            _mapper = mapper;
        }

        //[Authorize(Roles = "Admin,Customer manager")]
        [Authorize(Policy = "ORDER")]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderByIdForAdmin(int orderId)
        {
            var order = await _ordersService.GetOrderByIdAsync(orderId);
            if (order == null)
                return NotFound();
            else
            {
                var orderreturn = _mapper.Map<OrderForDetailDto>(order);

                decimal discount = (decimal)(orderreturn.Total1 * orderreturn.Discount / 100);
                orderreturn.Pay = orderreturn.Total1 - discount;
                orderreturn.Total2 = (decimal)(orderreturn.Pay + orderreturn.ShippingFee);
                return Ok(orderreturn);
            
            }
        }
        [Authorize(Policy = "ORDER")]
        // [Authorize(Roles = "Admin,Customer manager")]
        public IActionResult GetAllOrder(string keyword, int page = 1, int pageSize = 10, int sort = 0, string criteria = "OrderId")
        {
            try
            {

                var list = _ordersService.GetOders(keyword);

                var listforDto = _mapper.Map<IEnumerable<Orders>, IEnumerable<OrderForListDto>>(list);
                int totalCount = list.Count();               
                var response = _ordersService.GetOrdersPerPage(listforDto, page, pageSize, sort, criteria);

                var paginationSet = new PaginationSet<OrderForListDto>()
                {
                    Total = totalCount,
                    Items = response
                };

                return Ok(paginationSet);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
        [Authorize(Policy = "ORDER")]
        //[Authorize(Roles = "Admin,Customer manager")]
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] OrderForUpdateDto input)
        {
            if (ModelState.IsValid)
            {
                var orderInDB = await _ordersService.GetOrderByIdAsync(orderId);
                if (orderInDB == null)
                {
                    return NotFound(orderId);
                }
                var result = await _ordersService.UpdateOrderAsync(_mapper.Map(input, orderInDB));
                if (result)
                {
                    return Ok();
                }
            }
            return BadRequest(ModelState);
        }
        [Authorize(Policy = "ORDER")]
        //[Authorize(Roles = "Admin,Customer manager")]
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var order = await _ordersService.GetOrderByIdAsync(orderId);
            if (order == null)
                return NotFound(orderId);
            var result = await _ordersService.DeleteOrderAsync(order.OrderID);
            if (!result)
            {
                return BadRequest("Có lỗi trong quá trình xóa dữ liệu: ");
            }
            return Ok();
        }

        [Authorize(Roles = "User")]
        [HttpGet("user")]
        public async Task<IActionResult> GetOrder()
        {
            var userId = GetUserId();
            if (userId == "error")
            {
                return Unauthorized();
            }
            var order = await _ordersService.GetOrder(userId);
            var orderForReturn = _mapper.Map<IEnumerable<Orders>, IEnumerable<OrderForUserListDto>>(order);
            if (order == null)
                return NotFound();
            return Ok(new { data = orderForReturn });
        }

        [Authorize(Roles = "User")]
        [HttpGet("user/{orderId}")]
        public async Task<IActionResult> GetOrderByIdForUser(int orderId)
        {
            var order = await _ordersService.GetOrderByIdForUserAsync(orderId);
            if (order == null)
                return NotFound(orderId);
            else
            {
                var orderreturn = _mapper.Map<OrderForUserDetailDto>(order);
                decimal a = (decimal)(orderreturn.TamTinh + orderreturn.ShippingFee);
                orderreturn.Total = a - orderreturn.Discount;
                return Ok(orderreturn);
            }

        }
        [Authorize(Roles = "Admin,Book manager,Customer manager")]
        [HttpGet("statistic/all")]
        public IActionResult GetOrderCount(string from, string to)
        {
            try
            {
                if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
                {
                    var dateStarted = DateTime.ParseExact(from, "d/M/yyyy",
                     CultureInfo.CreateSpecificCulture("fr-FR"));
                    var dateEnded = DateTime.ParseExact(to, "d/M/yyyy",
                          CultureInfo.CreateSpecificCulture("fr-FR"));
                    var countFromTo = _ordersService.GetOders(null)
                                    .Where(x => x.Date >= dateStarted
                                    && x.Date <= dateEnded).Count();
                    return Ok(new { orderCount = countFromTo });
                }
                var orders = _ordersService.GetOders(null);
                return Ok(new { orderCount = orders.Count() });
            }
            catch(System.Exception)
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin,Book manager,Customer manager")]
        [HttpGet("statistic/completed")]
        public IActionResult GetCompletedOrderCount(string from, string to)
        {
            try
            {
                if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
                {
                    var dateStarted = DateTime.ParseExact(from, "d/M/yyyy",
                     CultureInfo.CreateSpecificCulture("fr-FR"));
                    var dateEnded = DateTime.ParseExact(to, "d/M/yyyy",
                          CultureInfo.CreateSpecificCulture("fr-FR"));
                    var countFromTo = _ordersService.GetOders(null)
                                    .Where(x => x.Date >= dateStarted 
                                    && x.Date <= dateEnded
                                    && x.Status.ToLower() == "Đã hoàn thành".ToLower()).Count();
                    return Ok(new { orderCount = countFromTo });
                }
                var orders = _ordersService.GetOders(null);
                return Ok(new { orderCount = orders.Count() });
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin,Book manager,Customer manager")]
        [HttpGet("statistic/total")]
        public IActionResult GetOrderPriceTotal(string from, string to)
        {
            try
            {
                var count = _ordersService.GetPriceTotal(from, to).GetAwaiter().GetResult();
                return Ok(new { orderCount = count });
            }
            catch (System.Exception)
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
