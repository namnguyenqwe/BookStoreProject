using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreProject.Helpers;
using BookStoreProject.Models;
using BookStoreProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class SubcribersController : ControllerBase
    {
        private readonly ISubcriberService _subcriberService;
        public SubcribersController(ISubcriberService subcriberService)
        {
            _subcriberService = subcriberService;
        }
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
        [HttpGet]
        public async Task<IActionResult> GetSubcriberById(int subcriberId)
        {
            var subcriber = await  _subcriberService.GetSubcriberById(subcriberId);
            if (subcriber == null)
                return NotFound(subcriberId);
            return Ok(subcriber);
        }
    }
}
