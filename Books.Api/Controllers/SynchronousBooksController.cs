using Books.API.Services;
using Books.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Books.API.Controllers
{
    [Route("api/synchronousbooks")]
    [ApiController]
    public class SynchronousBooksController : ControllerBase
    {
        private readonly IBooksRepository booksRepository = null;

        public SynchronousBooksController(IBooksRepository booksRepository)
        {
            this.booksRepository = booksRepository;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            IEnumerable<Book> result = this.booksRepository.GetBooksAsync().Result;
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetBook(Guid id)
        {
            Book result = this.booksRepository.GetBookAsync(id).Result;
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
