using System.Collections.Generic;
using System.Threading.Tasks;
using TodoAPI.Repository.Core.Contracts;
using TodoAPI.Repository.Core.Exceptions;
using TodoAPI.Repository.Core.Models;
using TodoAPI.Services.Core.Contracts;
using TodoAPI.Services.Core.Exceptions;
using TodoAPI.Services.Core.Models;

namespace TodoAPI.Services.Foundation.Services
{
    public class TodosService: ITodosService
    {
        private readonly IMapper _mapper;
        private readonly ITodosRepository _todosRepository;

        public TodosService(IMapper mapper, ITodosRepository todosRepository)
        {
            _mapper = mapper;
            _todosRepository = todosRepository;
        }

        public async Task<IEnumerable<TodoDTO>> GetAllTodosAsync()
        {
            var todos = await _todosRepository.GetAllTodosAsync();
            return _mapper.Map<IEnumerable<Todo>, IEnumerable<TodoDTO>>(todos);
        }

        public async Task<TodoDTO> FindByIdAsync(long id)
        {
            var todo = await _todosRepository.FindByIdAsync(id);
            return _mapper.Map<Todo, TodoDTO>(todo);
        }

        public async Task<TodoDTO> CreateTodoAsync(CreatedTodoDTO createdTodoDTO)
        {
            var todoToCreate = _mapper.Map<CreatedTodoDTO, Todo>(createdTodoDTO);
            var todoToReturn = _mapper.Map<CreatedTodoDTO, TodoDTO>(createdTodoDTO);

            todoToReturn.Id = await _todosRepository.CreateTodoAsync(todoToCreate);

            return todoToReturn;
        }

        public async Task DeleteTodoByIdAsync(long id)
        {
            await _todosRepository.DeleteTodoByIdAsync(id);
        }

        public async Task UpdateTodoAsync(UpdatedTodoDTO updatedTodoDTO)
        {
            var todoToUpdate = _mapper.Map<UpdatedTodoDTO, Todo>(updatedTodoDTO);

            try
            {
                await _todosRepository.UpdateTodoAsync(todoToUpdate);
            }
            catch (RecordNotFoundException)
            {
                throw new TodoNotFoundException();
            }
            catch (ConcurrentAccessException)
            {
                // TODO: Add logging here to indicate that we have hit
                // a database concurrency issue. For now I'll just
                // throw a NotFound error if the todo does not actually exist.
                // If it does exist, for now I'll just rethrow the error.
                bool todoExists = await _todosRepository.Exists(updatedTodoDTO.Id.Value);

                if (!todoExists)
                {
                    throw new TodoNotFoundException();
                }

                throw;
            }
        }
    }
}
