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

        [Authorize(Roles = "Admin")]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderByIdForAdmin(int orderId)
        {
            var order = await _ordersService.GetOrderByIdAsync(orderId);
            if (order == null)
                return NotFound();
            else return Ok(_mapper.Map<OrderForDetailDto>(order));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult GetAllOrder(string keyword, int page = 1, int pageSize = 10, int sort = 0, string criteria = "OrderId")
        {
            try
            {

                var list = _ordersService.GetOders(keyword);

                var listforDto = _mapper.Map<IEnumerable<Orders>, IEnumerable<OrderForListDto>>(list);
                int totalCount = list.Count();               
                var response = _ordersService.GetBooksPerPage(listforDto, page, pageSize, sort, criteria);

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

    }
}
