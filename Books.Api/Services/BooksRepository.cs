using Books.Api.Contexts;
using Books.Api.Entities;
using Books.Api.ExternalModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Books.Api.Services
{
    public class BooksRepository : IBooksRepository, IDisposable
    {
        private BooksContext context = null;
        private readonly IHttpClientFactory httpClientFactory = null;
        private CancellationTokenSource cancellationTokenSource = null;
        private ILogger<BooksRepository> logger = null;

        public BooksRepository(BooksContext context, IHttpClientFactory httpClientFactory, ILogger<BooksRepository> logger)
        {
            this.context = context;
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
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

        public async Task<BookCover> GetBookCoverAsync(string coverId)
        {
            using (HttpClient httpClient = this.httpClientFactory.CreateClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync($"http://localhost:52644/api/bookcovers/{coverId}");
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<BookCover>(await response.Content.ReadAsStringAsync());
                }
                return null;
            }
        }

        public async Task<IEnumerable<BookCover>> GetBookCoversAsync(Guid bookId)
        {
            using (HttpClient httpClient = this.httpClientFactory.CreateClient())
            {
                List<BookCover> bookCovers = new List<BookCover>();
                this.cancellationTokenSource = new CancellationTokenSource();

                string[] bookCoverUrls = new[]
                {
                    $"http://localhost:52644/api/bookcovers/{bookId}-dummycover1",
                    $"http://localhost:52644/api/bookcovers/{bookId}-dummycover2?returnFault=true",
                    $"http://localhost:52644/api/bookcovers/{bookId}-dummycover3",
                    $"http://localhost:52644/api/bookcovers/{bookId}-dummycover4",
                    $"http://localhost:52644/api/bookcovers/{bookId}-dummycover5"
                };

                List<Task<BookCover>> downloadBookCoverTasks = bookCoverUrls
                    .Select(url => this.DownloadBookCoverAsync(httpClient, url, cancellationTokenSource.Token)).ToList();

                try
                {
                    return await Task.WhenAll(downloadBookCoverTasks);
                }
                catch (OperationCanceledException operationCanceledException)
                {
                    this.logger.LogInformation($"{operationCanceledException.Message}");
                    foreach(Task task in downloadBookCoverTasks)
                    {
                        this.logger.LogInformation($"Task {task.Id} has status {task.Status}");
                    }
                    return new List<BookCover>();
                }
            }
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

        private async Task<BookCover> DownloadBookCoverAsync(HttpClient httpClient, string bookCoverUrl, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await httpClient.GetAsync(bookCoverUrl, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                BookCover bookCover = JsonConvert.DeserializeObject<BookCover>(
                    await response.Content.ReadAsStringAsync());
                return bookCover;
            }

            this.cancellationTokenSource.Cancel();
            return null;
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

                if (this.cancellationTokenSource != null)
                {
                    this.cancellationTokenSource.Dispose();
                    this.cancellationTokenSource = null;
                }
            }
        }
    }
}
