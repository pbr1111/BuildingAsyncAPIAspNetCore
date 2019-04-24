using Books.Api.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Books.Api.DependencyInjection
{
    public static class ConfigurationServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration["ConnectionStrings:BooksDBConnectionString"];
            services.AddDbContext<BooksContext>(o => o.UseSqlServer(connectionString));

            return services;
        }
    }
}
