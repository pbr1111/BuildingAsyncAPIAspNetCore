using Books.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Books.Data.Services
{
    public interface IBooksRepository
    {
        IEnumerable<Entities.Book> GetBooks();
        void AddBook(Entities.Book bookToAdd);
        Task<IEnumerable<Entities.Book>> GetBooksAsync();
        Task<IEnumerable<Entities.Book>> GetBooksAsync(IEnumerable<Guid> bookIds);
        Task<BookCover> GetBookCoverAsync(string coverId);
        Task<IEnumerable<BookCover>> GetBookCoversAsync(Guid bookId);
        Task<Entities.Book> GetBookAsync(Guid id);
        Task<bool> SaveChangesAsync();
    }
}
