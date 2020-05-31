using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.Repository.EntityFramework.Contexts;
using TodoAPI.Repository.Core.Contracts;
using TodoAPI.Repository.Core.Exceptions;
using TodoAPI.Repository.Core.Models;

namespace TodoAPI.Repository.EntityFramework.Repositories
{
    public class TodosRepository: ITodosRepository
    {
        private readonly TodoContext _context;

        public TodosRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<long> CreateTodoAsync(Todo todo)
        {
            _context.Todos.Add(todo);

            await _context.SaveChangesAsync();

            return todo.Id;
        }

        public async Task DeleteTodoByIdAsync(long id)
        {
            var todo = await _context.Todos.FindAsync(id);

            // TODO: Think about whether we should throw
            //       an error if todo was not found.
            if (todo != null)
            {
                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> Exists(long id)
        {
            return await FindByIdAsync(id) != null;
        }

        public async Task<Todo> FindByIdAsync(long id)
        {
            return await _context.Todos.FindAsync(id);
        }

        public async Task<IEnumerable<Todo>> GetAllTodosAsync()
        {
            return await _context.Todos.ToListAsync();
        }

        public async Task UpdateTodoAsync(Todo todo)
        {
            var todoToUpdate = await FindByIdAsync(todo.Id);

            if (todoToUpdate == null)
            {
                throw new RecordNotFoundException();
            }

            todoToUpdate.Name = todo.Name;
            todoToUpdate.IsComplete = todo.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ConcurrentAccessException();
            }
        }
    }
}
