using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreProject.Dtos.Recipient;
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
    public class RecipientsController : ControllerBase
    {
        private readonly IRecipientService _recipientService;
        private readonly IMapper _mapper;
        public RecipientsController(IRecipientService recipientService, IMapper mapper)
        {
            _recipientService = recipientService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetRecipientsByUserId()
        {
            try
            {
                var userEmail = GetUserEmail();
                if (userEmail == "error")
                {
                    return Unauthorized();
                }
                var recipients = await _recipientService.GetRecipientsByEmail(userEmail);
                var recipientsForReturn = _mapper.Map<IEnumerable<Recipient>, IEnumerable<RecipientForUserListDto>>(recipients);
                return Ok(new { data = recipientsForReturn });
            }
            catch(System.Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet("{recipientId}")]
        public async Task<IActionResult> GeRecipientById(int recipientID)
        {
            var userEmail = GetUserEmail();
            if (userEmail == "error")
            {
                return Unauthorized();
            }
            var recipient = await _recipientService.GetRecipientById(recipientID,userEmail);
            if (recipient == null)
                return NotFound(new { message = "Không tìm thấy !" });
            return Ok(_mapper.Map<RecipientForUserDetailDto>(recipient));
        }
        [HttpGet("default")]
        public async Task<IActionResult> GetDefaultRecipient()
        {
            var userEmail = GetUserEmail();
            if (userEmail == "error")
            {
                return Unauthorized();
            }
            var recipient = await _recipientService.GetDefaultRecipient(userEmail);
            if (recipient == null)
                return NotFound(new { message = "Không tìm thấy người nhận" });
            return Ok(_mapper.Map<RecipientForDefaultDto>(recipient));
        }
        [HttpPost]
        public async Task<IActionResult> CreateRecipient([FromBody] RecipientForCreateDto input)
        {
            if (ModelState.IsValid)
            {
               // var userId = GetUserId();
                var userEmail = GetUserEmail();
                if (userEmail == "error")
                {
                    return Unauthorized(new { message = "Unauthorized" });
                }
                var recipient = _mapper.Map<Recipient>(input);
                //recipient.ApplicationUserID = userId;
                recipient.Email = userEmail;
                var result = await _recipientService.CreateRecipient(recipient);
                if (result)
                    return Ok(new { message = "Thêm người nhận thành công!" });
            }
            return BadRequest(new { message = "Invalid recipient" });
        }
        [HttpPut("{recipientId}")]
        public async Task<IActionResult> UpdateRecipient(int recipientId)
        {
            if (ModelState.IsValid)
            {
                var userEmail = GetUserEmail();
                if (userEmail == "error")
                {
                    return Unauthorized(new { message = "Unauthorized" });
                }
                var recipientInDB = await _recipientService.GetRecipientById(recipientId,userEmail);
                if (recipientInDB == null)
                {
                    return NotFound(recipientId);
                }
                recipientInDB.Status = false;
                //recipientUpdate.Email = userEmail;
                var result = await _recipientService.UpdateRecipient(recipientInDB);
                if (result)
                {
                    return Ok(new { message = "Thông tin người dùng này ko còn khả dụng !"});
                }
            }
            return BadRequest(new { message = ModelState.Values.First().Errors[0].ErrorMessage });
        }
       
        [NonAction]
        public string GetUserEmail()
        {
            string userEmail;
            try
            {
                userEmail = User.Claims.First(c => c.Type == "Email").Value;
            }
            catch
            {
                return "error";
            }
            return userEmail;
        }
    }
}
