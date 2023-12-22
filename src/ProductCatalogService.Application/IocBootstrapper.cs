using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ProductCatalogService.Application
{
    public static class IocBootstrapper
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}