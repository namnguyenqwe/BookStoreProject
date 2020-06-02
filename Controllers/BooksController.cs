using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreProject.Dtos.Book;
using BookStoreProject.Helpers;
using BookStoreProject.Models;
using BookStoreProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private IBookService _bookService;
        private readonly IMapper _mapper;
        public BooksController(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }
        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBookByIdForAdmin(int bookId)
        {
            var book = await _bookService.GetBookByIdAsync(bookId);
            if (book == null)
                return NotFound();
            else return Ok(_mapper.Map<BookForDetailDto>(book));
        }
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            var book = await _bookService.GetBookByIdAsync(bookId);
            if (book == null)
                return NotFound(bookId);
            var result = await _bookService.DeleteBookAsync(book.BookID);
            if (!result)
            {
                return BadRequest("Có lỗi trong quá trình xóa dữ liệu: ");
            }
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookForCreateDto input)
        {
            if (ModelState.IsValid)
            {
                var book = _mapper.Map<Book>(input);
                var result = await _bookService.CreateBookAsync(book);
                if (result)
                    return Ok();
            }
            return BadRequest(ModelState);
        }
        [HttpPut("{bookId}")]
        public async Task<IActionResult> UpdateBook(int bookId, [FromBody] BookForDetailDto input)
        {
            if (ModelState.IsValid)
            {
                var bookInDB = await _bookService.GetBookByIdAsync(bookId);
                if (bookInDB == null)
                {
                    return NotFound(bookId);
                }
                var result = await _bookService.UpdateBookAsync(_mapper.Map(input, bookInDB) );
                if (result)
                {
                    return Ok();
                }    
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        public IActionResult GetAllBooks(string keyword, int page = 1, int pageSize = 10, int sort = 0, string criteria = "BookId")
        {
            
            try
            {
                
                var list = _bookService.GetBooks(keyword);
                return Ok(list);
                var listforDto = _mapper.Map<IEnumerable<Book>, IEnumerable<BookForListDto>>(list);
                int totalCount = list.Count();

                var response = _bookService.GetBooksPerPage(listforDto, page, pageSize, sort, criteria);

                var paginationSet = new PaginationSet<BookForListDto>()
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
        
    }
}
