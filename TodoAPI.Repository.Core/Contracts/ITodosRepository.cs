using System.Collections.Generic;
using System.Threading.Tasks;
using TodoAPI.Repository.Core.Models;

namespace TodoAPI.Repository.Core.Contracts
{
    public interface ITodosRepository
    {
        Task<IEnumerable<Todo>> GetAllTodosAsync();
        Task<Todo> FindByIdAsync(long id);
        Task<bool> Exists(long id);
        Task<long> CreateTodoAsync(Todo todo);
        Task DeleteTodoByIdAsync(long id);
        Task UpdateTodoAsync(Todo todo);
    }
}
