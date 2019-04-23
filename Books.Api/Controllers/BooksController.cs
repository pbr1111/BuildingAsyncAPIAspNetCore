using Books.Api.Entities;
using Books.Api.Filters;
using Books.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Books.Api.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private IBooksRepository booksRepository = null;

        public BooksController(IBooksRepository booksRepository)
        {
            this.booksRepository = booksRepository 
                ?? throw new ArgumentNullException(nameof(booksRepository));
        }

        [HttpGet]
        [BooksResultFilter]
        public async Task<IActionResult> GetBooks()
        {
            IEnumerable<Book> result = await this.booksRepository.GetBooksAsync();
            return Ok(result);
        }

        [HttpGet]
        [BookResultFilter]
        [Route("{id}")]
        public async Task<IActionResult> GetBook(Guid id)
        {
            Book result = await this.booksRepository.GetBookAsync(id);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
