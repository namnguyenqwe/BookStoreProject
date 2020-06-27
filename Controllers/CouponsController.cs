using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreProject.Dtos.Coupon;
using BookStoreProject.Helpers;
using BookStoreProject.Models;
using BookStoreProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CouponsController : ControllerBase
    {
        private readonly ICouponService _couponService;
        private readonly IMapper _mapper;
        public CouponsController(ICouponService couponService, IMapper mapper)
        {
            _couponService = couponService;
            _mapper = mapper;
        }
        [Authorize(Policy = "COUPON")]
        [HttpGet]
        public IActionResult GetAllCoupons(string keyword, int page = 1, int pageSize = 10, int sort = 0, string criteria = "couponid")
        {
            try
            {
                var list = _couponService.GetCoupons(keyword);
                int totalCount = list.Count();
                criteria = criteria.ToLower();
                var response = _mapper.Map<IEnumerable<Coupon>, IEnumerable<CouponForListDto>>(list);

                #region Sort by criteria
                if (criteria.Equals("couponid"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.CouponID).Skip((page - 1) * pageSize).Take(pageSize);

                    else response = response.OrderBy(x => x.CouponID).Skip((page - 1) * pageSize).Take(pageSize);
                }

                if (criteria.Equals("discount"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.Discount).Skip((page - 1) * pageSize).Take(pageSize);

                    else response = response.OrderBy(x => x.Discount).Skip((page - 1) * pageSize).Take(pageSize);
                }

                if (criteria.Equals("quantity"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.Quantity).Skip((page - 1) * pageSize).Take(pageSize);

                    else response = response.OrderBy(x => x.Quantity).Skip((page - 1) * pageSize).Take(pageSize);
                }

                if (criteria.Equals("quantityused"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.QuantityUsed).Skip((page - 1) * pageSize).Take(pageSize);

                    else response = response.OrderBy(x => x.QuantityUsed).Skip((page - 1) * pageSize).Take(pageSize);
                }

                if (criteria.Equals("status"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.Status).Skip((page - 1) * pageSize).Take(pageSize);

                    else response = response.OrderBy(x => x.Status).Skip((page - 1) * pageSize).Take(pageSize);
                }
                #endregion

                var paginationSet = new PaginationSet<CouponForListDto>()
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
        [Authorize(Policy = "COUPON")]
        [HttpGet("{couponId}")]
        public async Task<IActionResult> GetCouponById(string couponId)
        {
            var coupon = await _couponService.GetCouponById(couponId);
            if (coupon == null)
                return NotFound(couponId);
            return Ok(_mapper.Map<CouponForListDto>(coupon));
        }
        [Authorize(Policy = "COUPON")]
        [HttpPost]
        public async Task<IActionResult> CreateCoupon([FromBody] CouponForModalDto input)
        {
            if (ModelState.IsValid)
            {
                var couponInDB = await _couponService.GetCouponById(input.CouponID);
                if (couponInDB != null)
                    return BadRequest(new { message = "Coupon already exists" });
                var coupon = _mapper.Map<Coupon>(input);
                var result = await _couponService.CreateCoupon(coupon);
                if (result)
                    return Ok();
            }
            return BadRequest(new { message = ModelState.Values.First().Errors[0].ErrorMessage });
        }
        [Authorize(Policy = "COUPON")]
        [HttpPut("{couponId}")]
        public async Task<IActionResult> UpdateCoupon(string couponId, [FromBody] CouponForModalDto input)
        {
            if (ModelState.IsValid)
            {
                var couponInDB = await _couponService.GetCouponById(couponId);
                if (couponInDB == null)
                    return NotFound(couponId);

                var result = await _couponService.UpdateCoupon(_mapper.Map(input, couponInDB));
                if (result)
                {
                    return Ok();
                }
            }
            return BadRequest(ModelState);
        }
        [Authorize(Roles = "User")]
        [HttpGet("check/{couponId}")]
        public async Task<IActionResult> CheckCoupon(string couponId)
        {
            var coupon = await _couponService.IsCouponAvailable(couponId);
            if (coupon == null)
                return NotFound(new { message = "Mã giảm giá không còn khả dụng hoặc không tồn tại" });
            return Ok(_mapper.Map<CouponForPaymentDto>(coupon));
        }
    }
}
