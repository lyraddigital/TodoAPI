using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

namespace TodoAPI.Services.Configuration
{
    public static class MappingServiceConfiguration
    {
        public static void AddMappingService(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Foundation.Mapping.MappingProfile));
            services.AddTransient<Core.Contracts.IMapper, Foundation.Mapping.Mapper>();
        }
    }
}
