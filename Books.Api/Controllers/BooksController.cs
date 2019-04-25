using AutoMapper;
using Books.Data.Services;
using Books.Entities.DTO;
using Books.Entities.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Books.API.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository booksRepository = null;
        private readonly IMapper mapper = null;

        public BooksController(IBooksRepository booksRepository, IMapper mapper)
        {
            this.booksRepository = booksRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [BooksResultFilter]
        public async Task<IActionResult> GetBooks()
        {
            IEnumerable<Entities.Book> result = await this.booksRepository.GetBooksAsync();
            return Ok(result);
        }

        [HttpGet]
        [BookWithCoversResultFilter]
        [Route("{id}", Name = "GetBook")]
        public async Task<IActionResult> GetBook(Guid id)
        {
            Entities.Book bookEntity = await this.booksRepository.GetBookAsync(id);
            if (bookEntity == null)
            {
                return NotFound();
            }

            IEnumerable<BookCover> bookCovers = await this.booksRepository.GetBookCoversAsync(id);
            
            return Ok((bookEntity, bookCovers));
        }

        [HttpPost]
        [BookResultFilter]
        public async Task<IActionResult> CreateBook([FromBody] BookForCreation book)
        {
            Entities.Book bookEntity = this.mapper.Map<Entities.Book>(book);
            this.booksRepository.AddBook(bookEntity);

            await this.booksRepository.SaveChangesAsync();

            await this.booksRepository.GetBookAsync(bookEntity.Id);

            return CreatedAtRoute("GetBook", new { id = bookEntity.Id }, bookEntity);
        }
    }
}
