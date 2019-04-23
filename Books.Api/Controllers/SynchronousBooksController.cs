using Books.Api.Entities;
using Books.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Books.Api.Controllers
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
            IEnumerable<Book> result = this.booksRepository.GetBooks();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetBook(Guid id)
        {
            Book result = this.booksRepository.GetBook(id);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
