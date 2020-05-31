using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Controllers;
using TodoAPI.Services.Core.Contracts;
using TodoAPI.Services.Core.Exceptions;
using TodoAPI.Services.Core.Models;
using Xunit;

namespace TodoAPI.Tests.Controllers
{
    public class TodosControllerTests
    {
        [Fact]
        public async Task GetTodos_ReturnsAnOkResultWithTodosArray()
        {
            // Arrange
            var expectedTodos = new List<TodoDTO>() {
                new TodoDTO() { Id = 1, Name = "Todo 1", IsComplete = true }
            };

            var mockService = new Mock<ITodosService>();
            mockService.Setup(ts => ts.GetAllTodosAsync()).ReturnsAsync(expectedTodos);

            var controller = new TodosController(mockService.Object);

            // Action
            var actualTodos = await controller.GetTodos();

            // Assert
            Assert.NotNull(actualTodos);
            Assert.Single(actualTodos);
            Assert.Equal(expectedTodos[0].Id, actualTodos.ToList()[0].Id);
            Assert.Equal(expectedTodos[0].Name, actualTodos.ToList()[0].Name);
            Assert.Equal(expectedTodos[0].IsComplete, actualTodos.ToList()[0].IsComplete);
        }

        [Fact]
        public async Task GetTodo_TodoDoesNotExist_ReturnsANotFoundResult()
        {
            // Arrange
            var todoId = 1;
            var mockService = new Mock<ITodosService>();
            mockService.Setup(ts => ts.FindByIdAsync(todoId)).ReturnsAsync((TodoDTO)null);

            var controller = new TodosController(mockService.Object);

            // Action
            var result = await controller.GetTodo(todoId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetTodo_TodoExists_ReturnsOkResult()
        {
            // Arrange
            var todoId = 1;
            var expectedTodo = new TodoDTO() { Id = todoId, Name = "Todo 1", IsComplete = true };
            var mockService = new Mock<ITodosService>();
            mockService.Setup(ts => ts.FindByIdAsync(todoId)).ReturnsAsync(expectedTodo);

            var controller = new TodosController(mockService.Object);

            // Action
            var result = await controller.GetTodo(todoId);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(expectedTodo.Id, result.Value.Id);
            Assert.Equal(expectedTodo.Name, result.Value.Name);
            Assert.Equal(expectedTodo.IsComplete, result.Value.IsComplete);
        }

        [Fact]
        public async Task UpdateTodo_PayloadMissing_ReturnsBadRequest()
        {
            // Arrange
            var todoId = 1;
            var mockService = new Mock<ITodosService>();

            var controller = new TodosController(mockService.Object);

            // Action
            var result = await controller.UpdateTodo(todoId, null);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateTodo_ModelStateError_ReturnsBadRequest()
        {
            // Arrange
            var todoId = 1;
            var updatedTodoDTO = new UpdatedTodoDTO() { Name = "Todo 1", IsComplete = true };
            var mockService = new Mock<ITodosService>();

            var controller = new TodosController(mockService.Object);

            // Action
            controller.ModelState.AddModelError("updatedTodoDTO.Id", "ModelState error");
            var result = await controller.UpdateTodo(todoId, updatedTodoDTO);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateTodo_IdOnRouteDoesNotMatchPayload_ReturnsBadRequest()
        {
            // Arrange
            var todoId = 1;
            var updatedTodoDTO = new UpdatedTodoDTO() { Id = 2, Name = "Todo 1", IsComplete = true };
            var mockService = new Mock<ITodosService>();

            var controller = new TodosController(mockService.Object);

            // Action
            var result = await controller.UpdateTodo(todoId, updatedTodoDTO);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateTodo_TodoNotFoundExceptionThrow_ReturnsNotFoundResult()
        {
            // Arrange
            var todoId = 1;
            var updatedTodoDTO = new UpdatedTodoDTO() { Id = todoId, Name = "Todo 1", IsComplete = true };
            var mockService = new Mock<ITodosService>();

            mockService.Setup(ts => ts.UpdateTodoAsync(updatedTodoDTO)).Throws<TodoNotFoundException>();

            var controller = new TodosController(mockService.Object);

            // Action
            var result = await controller.UpdateTodo(todoId, updatedTodoDTO);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            mockService.Verify(ts => ts.UpdateTodoAsync(updatedTodoDTO), Times.Once());
        }

        [Fact]
        public async Task UpdateTodo_ValidTodoToUpdate_ReturnsNoContentResult()
        {
            // Arrange
            var todoId = 1;
            var updatedTodoDTO = new UpdatedTodoDTO() { Id = todoId, Name = "Todo 1", IsComplete = true };
            var mockService = new Mock<ITodosService>();

            mockService.Setup(ts => ts.UpdateTodoAsync(updatedTodoDTO));

            var controller = new TodosController(mockService.Object);

            // Action
            var result = await controller.UpdateTodo(todoId, updatedTodoDTO);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
            mockService.Verify(ts => ts.UpdateTodoAsync(updatedTodoDTO), Times.Once());
        }

        [Fact]
        public async Task CreateTodo_PayloadMissing_ReturnsBadRequest()
        {
            // Arrange
            var mockService = new Mock<ITodosService>();

            var controller = new TodosController(mockService.Object);

            // Action
            var result = await controller.CreateTodo(null);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateTodo_ModelStateError_ReturnsBadRequest()
        {
            // Arrange
            var createdTodoDTO = new CreatedTodoDTO() { IsComplete = true };
            var mockService = new Mock<ITodosService>();

            var controller = new TodosController(mockService.Object);

            // Action
            controller.ModelState.AddModelError("createdTodoDTO.Name", "ModelState error");
            var result = await controller.CreateTodo(createdTodoDTO);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateTodo_ValidData_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var createdTodoDTO = new CreatedTodoDTO();
            var expectedTodoDTO = new TodoDTO() { Id = 1, Name = "Some Todo", IsComplete = true };
            var mockService = new Mock<ITodosService>();

            mockService.Setup(ts => ts.CreateTodoAsync(createdTodoDTO)).ReturnsAsync(expectedTodoDTO);

            var controller = new TodosController(mockService.Object);

            // Action
            var result = await controller.CreateTodo(createdTodoDTO);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.IsType<TodoDTO>(((CreatedAtActionResult)result.Result).Value);

            var returnedResultTodo = (TodoDTO)((CreatedAtActionResult)result.Result).Value;

            Assert.Equal(expectedTodoDTO.Id, returnedResultTodo.Id);
            Assert.Equal(expectedTodoDTO.Name, returnedResultTodo.Name);
            Assert.Equal(expectedTodoDTO.IsComplete, returnedResultTodo.IsComplete);
        }

        [Fact]
        public async Task GetTodo_AllValid_ReturnsNoContentResult()
        {
            // Arrange
            var todoId = 1;
            var mockService = new Mock<ITodosService>();
            mockService.Setup(ts => ts.DeleteTodoByIdAsync(todoId));

            var controller = new TodosController(mockService.Object);

            // Action
            var result = await controller.DeleteTodo(todoId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
            mockService.Verify(ts => ts.DeleteTodoByIdAsync(todoId), Times.Once());
        }
    }
}
