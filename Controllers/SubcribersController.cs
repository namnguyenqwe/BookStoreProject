﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreProject.Dtos.Subcriber;
using BookStoreProject.Helpers;
using BookStoreProject.Models;
using BookStoreProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.VisualBasic.CompilerServices;

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class SubcribersController : ControllerBase
    {
        private readonly ISubcriberService _subcriberService;
        private readonly IMapper _mapper;
        public SubcribersController(ISubcriberService subcriberService, IMapper mapper)
        {
            _subcriberService = subcriberService;
            _mapper = mapper;
        }
        [Authorize(Policy = "SUBSCRIBER")]
        [HttpGet]
        public IActionResult GetAllSubcribers(string keyword, int page = 1, int pageSize = 10, int sort = 0, string criteria = "subcriberid")
        {
            try
            {
                var response = _subcriberService.GetSubcribers(keyword);
                int totalCount = response.Count();
                criteria = criteria.ToLower();
                

                #region Sort by criteria
                if (criteria.Equals("subcriberid"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.SubcriberId).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.SubcriberId).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else if (criteria.Equals("email"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.Email).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.Email).Skip((page - 1) * pageSize).Take(pageSize);
                }
                else //if (criteria.Equals("createdate"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.CreatedDate.Year)
                                               .ThenByDescending(x => x.CreatedDate.Month)
                                               .ThenByDescending(x => x.CreatedDate.Day)
                                               .Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.CreatedDate.Year)
                                               .ThenBy(x => x.CreatedDate.Month)
                                               .ThenBy(x => x.CreatedDate.Day)
                                               .Skip((page - 1) * pageSize).Take(pageSize);
                }
                #endregion

                var paginationSet = new PaginationSet<Subcriber>()
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
        [Authorize(Policy = "SUBSCRIBER")]
        [HttpGet("{subcriberId}")]
        public async Task<IActionResult> GetSubcriberById(int subcriberId)
        {
            var subcriber = await  _subcriberService.GetSubcriberById(subcriberId);
            if (subcriber == null)
                return NotFound(subcriberId);
            return Ok(subcriber);
        }
        [Authorize(Policy = "SUBSCRIBER")]
        [HttpPost]
        public async Task<IActionResult> CreateSubcriber([FromBody] SubcriberForModalDto input)
        {
            if (ModelState.IsValid)
            {
                var subcriber = _mapper.Map<Subcriber>(input);
                subcriber.CreatedDate = DateTime.Now;
                var result = await _subcriberService.CreateSubcriber(subcriber);
                if (result)
                    return Ok(new { message = "Subcriber created successfully !" });
                else return BadRequest(new { message = "Email already exists !" });
            }
            return BadRequest(new { message = ModelState.Values.First().Errors[0].ErrorMessage });
        }
        [Authorize(Policy = "SUBSCRIBER")]
        [HttpPut("{subcriberId}")]
        public async Task<IActionResult> UpdateSubcriber(int subcriberId, [FromBody] SubcriberForModalDto input)
        {
            if (ModelState.IsValid)
            {
                var subcriberInDB = await _subcriberService.GetSubcriberById(subcriberId);
                var result = await _subcriberService.UpdateSubcriber(_mapper.Map(input, subcriberInDB));
                if (result)
                    return Ok();
                else return BadRequest(new { message = "Email already exists !" });
            }
            return BadRequest(ModelState);
        }
        [Authorize(Policy = "SUBSCRIBER")]
        [HttpDelete("{subcriberId}")]
        public async Task<IActionResult> DeleteSubcriber(int subcriberId)
        {
            var subcriberInDB = await _subcriberService.GetSubcriberById(subcriberId);
            if (subcriberInDB == null)
                return NotFound(subcriberId);
            var result = await _subcriberService.DeleteSubcriber(subcriberInDB.SubcriberId);
            if (!result)
                return BadRequest(new { message = "Có lỗi trong quá trình xóa dữ liệu"});
            return Ok();
        }
        [Authorize(Roles = "Admin,Customer manager,Book manager")]
        [HttpGet("statistic/all")]
        public IActionResult GetSubcriberCount(string from, string to)
        {
            try
            {
                if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
                {
                    var dateStarted = DateTime.ParseExact(from, "d/M/yyyy",
                     CultureInfo.CreateSpecificCulture("fr-FR"));
                    var dateEnded = DateTime.ParseExact(to, "d/M/yyyy",
                          CultureInfo.CreateSpecificCulture("fr-FR"));
                    var countFromTo = _subcriberService.GetSubcribers(null)
                                    .Where(x => x.CreatedDate >= dateStarted
                                    && x.CreatedDate <= dateEnded).Count();
                    return Ok(new { subscriberCount = countFromTo });
                }
                var subcribers = _subcriberService.GetSubcribers(null);
                return Ok(new { subscriberCount = subcribers.Count() });
            }
            catch(System.Exception)
            {
                return BadRequest();
            }
        }
    }
}
