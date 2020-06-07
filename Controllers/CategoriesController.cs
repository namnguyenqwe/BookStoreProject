using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreProject.Dtos.Category;
using BookStoreProject.Helpers;
using BookStoreProject.Models;
using BookStoreProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
                var category = await _categoryService.FindCategoryByIdAsync(categoryId);
                if (category == null)
                    return NotFound(categoryId);
                var result = await _categoryService.DeleteCategoryByIdAsync(category.CategoryID);
                if (!result)
                {
                    return BadRequest("Có lỗi trong quá trình xóa dữ liệu: ");
                }
                return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryCreate)
        {
            if(ModelState.IsValid)
            {
                var category = _mapper.Map<Categories>(categoryCreate);
                var result = await _categoryService.CreateCategoryAsync(category);
                if (result)
                    return Ok();
            }
            return BadRequest(ModelState);
        }
        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] CategoryDto input)
        {
            if(ModelState.IsValid)
            {
                var categoryInDB = await _categoryService.FindCategoryByIdAsync(categoryId);
                if (categoryInDB == null)
                    return NotFound(categoryId);
                var result = await _categoryService.UpdateCategoryAsync(_mapper.Map(input, categoryInDB));
                if(result)
                {
                    return Ok();
                }    
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        public IActionResult GetAllCategories(string keyword, int page = 1, int pageSize = 10, int sort = 0, string criteria = "categoryid")
        {
            try
            {
                var list = _categoryService.GetCategories(keyword);
                int totalCount = list.Count();
                criteria = criteria.ToLower();
                IEnumerable<Categories> query = null;
                if (criteria.Equals("categoryid"))
                {
                    if (sort == 0) query = list.OrderByDescending(x => x.CategoryID).Skip((page - 1) * pageSize).Take(pageSize);
                    else query = list.OrderBy(x => x.CategoryID).Skip((page - 1) * pageSize).Take(pageSize);
                }
                if (criteria.Equals("category"))
                {
                    if (sort == 0) query = list.OrderByDescending(x => x.Category).Skip((page - 1) * pageSize).Take(pageSize);
                    else query = list.OrderBy(x => x.Category).Skip((page - 1) * pageSize).Take(pageSize);
                }
                var response = _mapper.Map<IEnumerable<Categories>, IEnumerable<CategoryForListDto>>(query);
                var paginationSet = new PaginationSet<CategoryForListDto>()
                {
                    Items = response,
                    Total = totalCount,
                };

                return Ok(paginationSet);
            }
            catch(System.Exception)
            {
                return BadRequest();
            }
        }
    }
}
