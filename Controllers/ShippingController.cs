using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreProject.Dtos.District;
using BookStoreProject.Helpers;
using BookStoreProject.Models;
using BookStoreProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingService _shippingService;
        private readonly IMapper _mapper;
        public ShippingController (IShippingService shippingService, IMapper mapper)
        {
            _shippingService = shippingService;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetAllShippingLocations(string keyword, int page = 1, int pageSize = 10, int sort = 0, string criteria = "districtid")
        {
            try
            {
                var list = _shippingService.GetShippingLocations(keyword);

                var response = _mapper.Map<IEnumerable<District>, IEnumerable<DistrictForListDto>>(list);
                int totalCount = list.Count();
                criteria = criteria.ToLower();

                #region Sort by criteria
                if (criteria.Equals("districtid"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.DistrictID).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.DistrictID).Skip((page - 1) * pageSize).Take(pageSize);
                }

                else if (criteria.Equals("district"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.district).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.district).Skip((page - 1) * pageSize).Take(pageSize);
                }

                else if (criteria.Equals("city"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.city).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.city).Skip((page - 1) * pageSize).Take(pageSize);
                }

                else //if (criteria.Equals("fee"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.Fee).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.Fee).Skip((page - 1) * pageSize).Take(pageSize);
                }
                #endregion

                var paginationSet = new PaginationSet<DistrictForListDto>()
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
        [HttpGet("{districtId}")]
        public async Task<IActionResult> GetShippingLocationById(string districtId)
        {
            var shippingLocation = await _shippingService.GetShippingLocationById(districtId);
            if (shippingLocation == null)
                return NotFound(districtId);
            return Ok(_mapper.Map<DistrictForListDto>(shippingLocation));
        }
        [HttpPut("{districtId}")]
        public async Task<IActionResult> UpdateShippingLocation(string districtId, [FromBody] DistrictForUpdateDto input)
        {
            if (ModelState.IsValid)
            {
                var districtInDB = await _shippingService.GetShippingLocationById(districtId);
                if (districtInDB == null)
                    return NotFound(districtId);
                var result = await _shippingService.UpdateShippingLocation(_mapper.Map(input, districtInDB));
                if (result)
                {
                    return Ok();
                }    
            }
            return BadRequest(ModelState);
        }
    }
}
