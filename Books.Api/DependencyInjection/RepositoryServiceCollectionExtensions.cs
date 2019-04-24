using Books.API.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Books.API.DependencyInjection
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddRespositoriesConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IBooksRepository, BooksRepository>();

            return services;
        }
    }
}
