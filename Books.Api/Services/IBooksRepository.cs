﻿using Books.Api.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Books.Api.Services
{
    public interface IBooksRepository
    {
        IEnumerable<Book> GetBooks();
        Book GetBook(Guid id);
        void AddBook(Entities.Book bookToAdd);

        Task<IEnumerable<Book>> GetBooksAsync();
        Task<IEnumerable<Book>> GetBooksAsync(IEnumerable<Guid> bookIds);
        Task<Book> GetBookAsync(Guid id);
        Task<bool> SaveChangesAsync();
    }
}
