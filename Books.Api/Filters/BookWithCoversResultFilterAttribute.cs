using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Books.Models;

namespace Books.API.Filters
{
    public class BookWithCoversResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            ObjectResult resultFromAction = context.Result as ObjectResult;
            if (resultFromAction?.Value == null
                || resultFromAction.StatusCode < 200
                || resultFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }

            IMapper mapper = context.HttpContext.RequestServices.GetService<IMapper>();

            var (book, bookCovers) = ((Entities.Book book, IEnumerable<BookCover> bookCovers))resultFromAction.Value;
            BookWithCovers bookWithCovers = mapper.Map<BookWithCovers>(book);
            resultFromAction.Value = mapper.Map(bookCovers, bookWithCovers);

            await next();
        }
    }
}
