using AutoMapper;
using Books.API.Filters;
using Books.API.Services;
using Books.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.API.Controllers
{
    [Route("api/bookcollections")]
    [ApiController]
    [BooksResultFilter]
    public class BookCollectionController : ControllerBase
    {
        private readonly IBooksRepository booksRepository = null;
        private readonly IMapper mapper = null;

        public BookCollectionController(IBooksRepository booksRepository, IMapper mapper)
        {
            this.booksRepository = booksRepository;
            this.mapper = mapper;
        }

        [HttpGet("({bookIds})", Name = "GetBookCollection")]
        public async Task<IActionResult> GetBookCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> bookIds)
        {
            IEnumerable<Entities.Book> bookEntities = await this.booksRepository.GetBooksAsync(bookIds);

            if (bookIds.Count() != bookEntities.Count())
            {
                return NotFound();
            }
            return Ok(bookEntities);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBookCollection([FromBody] IEnumerable<BookForCreation> bookCollection)
        {
            IEnumerable<Entities.Book> bookEntities = this.mapper.Map<IEnumerable<Entities.Book>>(bookCollection);
            foreach (Entities.Book bookEntity in bookEntities)
            {
                this.booksRepository.AddBook(bookEntity);
            }

            await this.booksRepository.SaveChangesAsync();

            IEnumerable<Entities.Book> booksToReturn = await this.booksRepository.GetBooksAsync(bookEntities.Select(b => b.Id).ToList());
            string bookIds = string.Join(",", booksToReturn.Select(a => a.Id));

            return CreatedAtRoute("GetBookCollection", new { bookIds }, booksToReturn);
        }
    }
}
