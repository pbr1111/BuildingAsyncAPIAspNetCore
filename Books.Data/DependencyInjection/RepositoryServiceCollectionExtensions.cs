using Books.Data.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Books.Data.DependencyInjection
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
