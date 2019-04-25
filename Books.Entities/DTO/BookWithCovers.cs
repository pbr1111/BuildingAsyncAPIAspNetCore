using System.Collections.Generic;

namespace Books.Entities.DTO
{
    public class BookWithCovers : Book
    {
        public IEnumerable<BookCover> BookCovers { get; set; } = new List<BookCover>();
    }
}
