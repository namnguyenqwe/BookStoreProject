﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreProject.Dtos.Publisher;
using BookStoreProject.Helpers;
using BookStoreProject.Models;
using BookStoreProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "PUBLISHER")]
    public class PublishersController : ControllerBase
    {
        private IPublisherService _publisherService;
        private readonly IMapper _mapper;
        public PublishersController(IPublisherService publisherService, IMapper mapper)
        {
            _publisherService = publisherService;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetAllPublishers(string keyword, int page = 1, int pageSize = 10, int sort = 0, string criteria = "publisherId")
        {
            try
            {
                var list = _publisherService.GetPublishers(keyword);
                int totalCount = list.Count();
                criteria = criteria.ToLower();
                var response = _mapper.Map<IEnumerable<Publisher>, IEnumerable<PublisherForListDto>>(list);
                if (response != null)
                {
                    foreach (var item in response)
                    {
                        item.BookTitleCount = _publisherService.CountBookTitleInPublisher(item.PublisherID);
                    }
                }

                #region sort by criteria
                if (criteria.Equals("publisherid"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.PublisherID).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.PublisherID).Skip((page - 1) * pageSize).Take(pageSize);
                }
                if (criteria.Equals("publisher"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.publisher).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.publisher).Skip((page - 1) * pageSize).Take(pageSize);
                }
                if (criteria.Equals("booktitlecount"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.BookTitleCount).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.BookTitleCount).Skip((page - 1) * pageSize).Take(pageSize);
                }
                #endregion

                var paginationSet = new PaginationSet<PublisherForListDto>()
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
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPublishers()
        {
            try
            {
                var list = await _publisherService.GetPublishers();
                var listForReturn = _mapper.Map<IEnumerable<Publisher>, IEnumerable<PublisherForSelectDto>>(list);
                return Ok(listForReturn);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet("{publisherId}")]
        public async Task<IActionResult> GetPublisherById(int publisherId)
        {
            var publisherInDB = await _publisherService.GetPublisherByIdAsync(publisherId);
            if (publisherInDB == null)
                return NotFound(publisherId);
            var publisherForReturn = _mapper.Map<Publisher, PublisherForListDto>(publisherInDB);
            publisherForReturn.BookTitleCount = _publisherService.CountBookTitleInPublisher(publisherId);
            return Ok(publisherForReturn);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePublishers([FromBody] PublisherDto input)
        {
            if (ModelState.IsValid)
            {
                var publisher = _mapper.Map<PublisherDto, Publisher>(input);
                var result = await _publisherService.CreatePublisherAsync(publisher);
                if (result)
                    return Ok();
            }
            return BadRequest(new { message = ModelState.Values.First().Errors[0].ErrorMessage });
        }
        [HttpPut("{publisherId}")]
        public async Task<IActionResult> UpdatePublisher(int publisherId, [FromBody] PublisherDto input)
        {
            if (ModelState.IsValid)
            {
                var publisherInDB = await _publisherService.GetPublisherByIdAsync(publisherId);
                if (publisherInDB == null)
                    return NotFound(publisherId);
                var result = await _publisherService.UpdatePublisherAsync(_mapper.Map(input, publisherInDB));
                if (result)
                {
                    return Ok();
                }    
            }
            return BadRequest(new { message = ModelState.Values.First().Errors[0].ErrorMessage });
        }
        [HttpDelete("{publisherId}")]
        public async Task<IActionResult> DeletePublisher(int publisherId)
        {
            var publisher = await _publisherService.GetPublisherByIdAsync(publisherId);
            if (publisher == null)
            {
                return NotFound(publisherId);
            }
            var result = await _publisherService.DeletePublisherAsync(publisherId);
            if (!result)
            {
                return BadRequest("Có lỗi trong quá trình xóa dữ liệu: ");
            }
            return Ok();
        }
        
    }
    
}
