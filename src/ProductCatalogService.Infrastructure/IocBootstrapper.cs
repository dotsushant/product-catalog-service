using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalogService.Application.Interfaces.Persistence;
using ProductCatalogService.Infrastructure.Persistence;
using System.Data;

namespace ProductCatalogService.Infrastructure
{
    public static class IocBootstrapper
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
            services.AddTransient<IDbConnection>(connection =>
                new SqliteConnection(configuration["ConnectionStrings:SqliteConnection"]));
        }
    }
}