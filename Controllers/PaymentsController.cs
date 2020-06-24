using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> GetPaymentList()
        {
            try
            {
                var userId = GetUserId();
                if (userId == "error")
                {
                    return StatusCode(201, new { message = "unauthorized" });
                }
                var list = await _paymentService.GetPaymentList(userId);
                return Ok(list);
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
