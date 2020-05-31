using Microsoft.EntityFrameworkCore;
using TodoAPI.Repository.Core.Models;

namespace TodoApi.Repository.EntityFramework.Contexts
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }

        public DbSet<Todo> Todos { get; set; }
    }
}