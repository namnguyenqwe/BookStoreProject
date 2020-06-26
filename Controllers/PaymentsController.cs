using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BookStoreProject.Dtos.Payment;
using BookStoreProject.Models;
using BookStoreProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentsController (IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }    
        [HttpGet]
        public async Task<IActionResult> GetPaymentList([Required] string bookIds)
        {
            try
            {
                string[] BookIds = Regex.Split(bookIds, @"\D+");
                var userId = GetUserId();
                if (userId == "error")
                {
                    return Unauthorized(new { message = "unauthorized" });
                }
                var list = await _paymentService.GetPaymentList(userId,BookIds);
                return Ok(list);
            }
            catch(System.Exception)
            {
                return BadRequest();
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentForBillDto input, [Required] string bookIds)
        {
            string[] BookIds = Regex.Split(bookIds, @"\D+");
            var userId = GetUserId();
            if (userId == "error")
            {
                return Unauthorized(new { message = "unauthorized" });
            }
            var order = new Orders()
            {
                RecipientID = input.RecipientID,
                CouponID = input.CouponID,
                Note = input.Note,
                ShippingFee = input.ShippingFee,
                ApplicationUserID = userId,
                Status = "Đang vận chuyển",
                Date = DateTime.Now
            };
            var result = await _paymentService.SavePaymentBill(order);

            if (!result)
                return BadRequest(new { message = "Có lỗi xảy ra, vui lòng thử lại !" });

            result = await _paymentService.SaveBillItems(order, BookIds);

            if (!result)
                return BadRequest(new { message = "Có lỗi xảy ra, vui lòng thử lại !" });

            return Ok(new { message = "Thanh toán thành công !" });
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
