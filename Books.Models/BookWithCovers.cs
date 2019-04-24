using System.Collections.Generic;

namespace Books.Models
{
    public class BookWithCovers : Book
    {
        public IEnumerable<BookCover> BookCovers { get; set; } = new List<BookCover>();
    }
}
