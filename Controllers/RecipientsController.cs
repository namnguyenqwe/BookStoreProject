using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreProject.Dtos.Recipient;
using BookStoreProject.Models;
using BookStoreProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                var userId = GetUserId();
                if (userId == "error")
                {
                    return Unauthorized();
                }
                var recipients = await _recipientService.GetRecipientsByApplicationUserId(userId);
                var recipientsForReturn = _mapper.Map<IEnumerable<Recipient>, IEnumerable<RecipientForUserListDto>>(recipients);
                return Ok(new { data = recipientsForReturn });
            }
            catch(System.Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateRecipient([FromBody] RecipientForCreateDto input)
        {
            if (ModelState.IsValid)
            {
                var userId = GetUserId();
                if (userId == "error")
                {
                    return Unauthorized();
                }
                var recipient = _mapper.Map<Recipient>(input);
                recipient.ApplicationUserID = userId;
                var result = await _recipientService.CreateRecipient(recipient);
                if (result)
                    return Ok();
            }
            return BadRequest(new { message = "Invalid review" });
        }
        [HttpPut("{recipientId}")]
        public async Task<IActionResult> UpdateRecipient(int recipientId, [FromBody] RecipientForCreateDto input)
        {
            if (ModelState.IsValid)
            {
                var userId = GetUserId();
                if (userId == "error")
                {
                    return Unauthorized();
                }
                var recipientInDB = await _recipientService.GetRecipientById(recipientId,userId);
                if (recipientInDB == null)
                {
                    return NotFound(recipientId);
                }
                var recipientUpdate = _mapper.Map(input, recipientInDB);
                recipientUpdate.ApplicationUserID = userId;
                var result = await _recipientService.UpdateRecipient(recipientUpdate);
                if (result)
                {
                    return Ok();
                }
            }
            return BadRequest(ModelState);
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
