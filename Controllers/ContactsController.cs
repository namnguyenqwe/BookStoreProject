using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreProject.Dtos.Contact;
using BookStoreProject.Helpers;
using BookStoreProject.Models;
using BookStoreProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;
        public ContactsController(IContactService contactService, IMapper mapper)
        {
            _contactService = contactService;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetAllContacts(string keyword, int page = 1, int pageSize = 10, int sort = 0, string criteria = "categoryid")
        {
            try
            {
                var list = _contactService.GetContacts(keyword);
                int totalCount = list.Count();
                criteria = criteria.ToLower();
                var response = _mapper.Map<IEnumerable<Contact>, IEnumerable<ContactForListDto>>(list);
               
                #region Sort by criteria
                if (criteria.Equals("contactid"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.ContactID).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.ContactID).Skip((page - 1) * pageSize).Take(pageSize);
                }

                if (criteria.Equals("name"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.Name).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.Name).Skip((page - 1) * pageSize).Take(pageSize);
                }

                if (criteria.Equals("message"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.Message).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.Message).Skip((page - 1) * pageSize).Take(pageSize);
                }

                if (criteria.Equals("date"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.Date.Year)
                                                .ThenByDescending(x => x.Date.Month)
                                                .ThenByDescending(x => x.Date.Day).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.Date.Year)
                                        .ThenBy(x => x.Date.Month)
                                        .ThenBy(x => x.Date.Day).Skip((page - 1) * pageSize).Take(pageSize);
                }

                else //if (criteria.Equals("status"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.Status).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.Status).Skip((page - 1) * pageSize).Take(pageSize);
                }
                #endregion

                var paginationSet = new PaginationSet<ContactForListDto>()
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
        [HttpGet("{contactId}")]
        public async Task<IActionResult> GetContactById(int contactId)
        {
            var contactInDB = await _contactService.GetContactById(contactId);
            if (contactInDB == null)
                return NotFound(contactId);
            return Ok(contactInDB);
        }
        [HttpPost]
        public async Task<IActionResult> CreateContact([FromBody] ContactForCreateDto input)
        {
            if (ModelState.IsValid)
            {
                var contact = _mapper.Map<Contact>(input);
                var result = await _contactService.CreateContact(contact);
                if (result)
                    return Ok();
            }
            return BadRequest(new { message = ModelState.Values.First().Errors[0].ErrorMessage });
        }
        [HttpDelete("{contactId}")]
        public async Task<IActionResult> DeleteContact(int contactId)
        {
            var contactInDB = await _contactService.GetContactById(contactId);
            if (contactInDB == null)
                return NotFound(contactId);
            var result = await _contactService.DeleteContact(contactInDB.ContactID);
            if (!result)
            {
                return BadRequest("Có lỗi trong quá trình xóa dữ liệu: ");
            }
            return Ok();
        }
        [HttpPut("{contactId}")]
        public async Task<IActionResult> UpdateContact(int contactId,[FromBody] ContactForUpdateDto input)
        {
            if (ModelState.IsValid)
            {
                var contactInDB = await _contactService.GetContactById(contactId);
                if (contactInDB == null)
                    return NotFound(contactId);
                var result = await _contactService.UpdateContact(_mapper.Map(input, contactInDB));
                if (result)
                {
                    return Ok();
                }
            }
            return BadRequest(new { message = ModelState.Values.First().Errors[0].ErrorMessage });
        }
    }
}
