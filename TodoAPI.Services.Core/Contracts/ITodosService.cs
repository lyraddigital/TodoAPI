using System.Collections.Generic;
using System.Threading.Tasks;
using TodoAPI.Services.Core.Models;

namespace TodoAPI.Services.Core.Contracts
{
    public interface ITodosService
    {
        Task<IEnumerable<TodoDTO>> GetAllTodosAsync();
        Task<TodoDTO> FindByIdAsync(long id);
        Task<TodoDTO> CreateTodoAsync(CreatedTodoDTO todoDTO);
        Task UpdateTodoAsync(UpdatedTodoDTO updatedTodoDTO);
        Task DeleteTodoByIdAsync(long id);
    }
}
