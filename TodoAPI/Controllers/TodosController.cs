using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.Services.Core.Models;
using TodoAPI.Services.Core.Contracts;
using TodoAPI.Services.Core.Exceptions;

namespace TodoAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodosService _todosService;

        public TodosController(ITodosService todosService)
        {
            _todosService = todosService;
        }

        [HttpGet]
        public async Task<IEnumerable<TodoDTO>> GetTodos()
        {
            return await _todosService.GetAllTodosAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoDTO>> GetTodo(long id)
        {
            var todoDTO = await _todosService.FindByIdAsync(id);

            if (todoDTO == null)
            {
                return NotFound();
            }

            return todoDTO;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTodo(long id, UpdatedTodoDTO updatedTodoDTO)
        {
            if (updatedTodoDTO == null)
            {
                return BadRequest("No todo found in payload");
                
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updatedTodoDTO.Id)
            {
                // TODO: Best to return an object and not a string, but for now
                // I'll at least do this.
                return BadRequest("The id in the route does not match the id in your payload");
            }

            try
            {
                await _todosService.UpdateTodoAsync(updatedTodoDTO);

                return NoContent();
            }
            catch(TodoNotFoundException)
            {
                return NotFound();
            }
            
        }

        [HttpPost]
        public async Task<ActionResult<TodoDTO>> CreateTodo(CreatedTodoDTO createdTodoDTO)
        {
            if (createdTodoDTO == null)
            {
                return BadRequest("No todo found in payload");

            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newTodoDTO = await _todosService.CreateTodoAsync(createdTodoDTO);

            return CreatedAtAction(nameof(GetTodo), new { id = newTodoDTO.Id }, newTodoDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(long id)
        {
            await _todosService.DeleteTodoByIdAsync(id);

            return NoContent();
        }
    }
}
