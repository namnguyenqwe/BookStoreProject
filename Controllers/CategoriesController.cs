using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
                var response = _mapper.Map<IEnumerable<Categories>, IEnumerable<CategoryForListDto>>(list);
                if (response != null)
                {
                    foreach (var item in response)
                    {
                        item.BookTitleCount = _categoryService.CountBookTitleInCategory(item.CategoryID);
                    }
                }

                #region Sort by criteria
                if (criteria.Equals("categoryid"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.CategoryID).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.CategoryID).Skip((page - 1) * pageSize).Take(pageSize);
                }
                if (criteria.Equals("category"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.Category).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.Category).Skip((page - 1) * pageSize).Take(pageSize);
                }
                if (criteria.Equals("booktitlecount"))
                {
                    if (sort == 0) response = response.OrderByDescending(x => x.BookTitleCount).Skip((page - 1) * pageSize).Take(pageSize);
                    else response = response.OrderBy(x => x.BookTitleCount).Skip((page - 1) * pageSize).Take(pageSize);
                }
                #endregion

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
        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            var categoryInDB = await _categoryService.FindCategoryByIdAsync(categoryId);
            if (categoryInDB == null)
                return NotFound(categoryId);
            var categoryForReturn = _mapper.Map<Categories, CategoryForListDto>(categoryInDB);
            categoryForReturn.BookTitleCount = _categoryService.CountBookTitleInCategory(categoryId);
            return Ok(categoryForReturn);
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categoriesInDB = await _categoryService.GetCategoriesAsync();
                var listForReturn = _mapper.Map<IEnumerable<Categories>,IEnumerable<CategoryForSelectDto>>(categoriesInDB);
                return Ok(listForReturn);
            }
            catch(System.Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet("user")]
        public async Task<IActionResult> GetAllCategoriesForUser()
        {
            try
            {
                var categoriesInDB = await _categoryService.GetCategoriesAsync();
                var listForReturn = _mapper.Map<IEnumerable<Categories>, IEnumerable<CategoryForUserListDto>>(categoriesInDB);
                return Ok(new { data = listForReturn });    
            }
            catch(System.Exception)
            {
                return BadRequest();
            }
        }
    }
}
