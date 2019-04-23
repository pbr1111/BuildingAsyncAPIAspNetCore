using Books.Api.Contexts;
using Books.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Api.Services
{
    public class BooksRepository : IBooksRepository, IDisposable
    {
        private BooksContext context = null;

        public BooksRepository(BooksContext context)
        {
            this.context = context;
        }

        public async Task<Book> GetBookAsync(Guid id)
        {
            await this.context.Database.ExecuteSqlCommandAsync("WAITFOR DELAY '00:00:02';");
            return await this.context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            await this.context.Database.ExecuteSqlCommandAsync("WAITFOR DELAY '00:00:02';");
            return await this.context.Books
                .Include(b => b.Author)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksAsync(IEnumerable<Guid> bookIds)
        {
            return await this.context.Books.Where(b => bookIds.Contains(b.Id))
                .Include(b => b.Author).ToListAsync();
        }

        public void AddBook(Book bookToAdd)
        {
            if (bookToAdd == null)
            {
                throw new ArgumentNullException(nameof(bookToAdd));
            }

            this.context.Add(bookToAdd);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await this.context.SaveChangesAsync() > 0);
        }

        public IEnumerable<Book> GetBooks()
        {
            this.context.Database.ExecuteSqlCommand("WAITFOR DELAY '00:00:02';");
            return this.context.Books
                .Include(b => b.Author)
                .ToList();
        }

        public Book GetBook(Guid id)
        {
            this.context.Database.ExecuteSqlCommand("WAITFOR DELAY '00:00:02';");
            return this.context.Books
                .Include(b => b.Author)
                .FirstOrDefault(b => b.Id == id);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.context != null)
                {
                    this.context.Dispose();
                    this.context = null;
                }
            }
        }
    }
}
