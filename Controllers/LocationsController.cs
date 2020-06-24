using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreProject.Dtos.City;
using BookStoreProject.Dtos.District;
using BookStoreProject.Models;
using BookStoreProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly IDistrictService _districtService;
        private readonly IMapper _mapper;
        public LocationsController (ICityService cityService, IDistrictService districtService, IMapper mapper)
        {
            _cityService = cityService;
            _districtService = districtService;
            _mapper = mapper;
        }
        [HttpGet("city")]
        public async Task<IActionResult> GetCities()
        {
            try
            {
                var citiesInDB = await _cityService.GetCities();
                var citiesForReturn = _mapper.Map<IEnumerable<City>, IEnumerable<CityForUserListDto>>(citiesInDB);
                return Ok(new { data = citiesForReturn });
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet("district/{cityId}")]
        public async Task<IActionResult> GetDistricts(string cityId)
        {
            try
            {
                var districtsInDB = await _districtService.GetDistrictsByCityId(cityId);
                var districtsForReturn = _mapper.Map<IEnumerable<District>, IEnumerable<DistrictForUserListDto>>(districtsInDB);
                return Ok(new { data = districtsForReturn });
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }    
    }
}
