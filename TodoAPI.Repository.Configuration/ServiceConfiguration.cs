using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TodoApi.Repository.EntityFramework.Contexts;
using TodoAPI.Repository.Core.Contracts;
using TodoAPI.Repository.EntityFramework.Repositories;

namespace TodoAPI.Repository.Configuration
{
    public static class ServiceConfiguration
    {
        public static void AddRepositoryServices(this IServiceCollection services)
        {
            services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
            services.AddTransient<ITodosRepository, TodosRepository>();
        }
    }
}
