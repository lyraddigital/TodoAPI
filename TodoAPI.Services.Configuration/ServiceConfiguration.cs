using Microsoft.Extensions.DependencyInjection;
using TodoAPI.Services.Core.Contracts;
using TodoAPI.Services.Foundation.Services;
using TodoAPI.Repository.Configuration;

namespace TodoAPI.Services.Configuration
{
    public static class ServiceConfiguration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddRepositoryServices();
            services.AddMappingService();

            services.AddTransient<ITodosService, TodosService>();
        }
    }
}
