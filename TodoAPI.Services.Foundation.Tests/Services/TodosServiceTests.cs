using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using TodoAPI.Repository.Core.Contracts;
using TodoAPI.Repository.Core.Exceptions;
using TodoAPI.Repository.Core.Models;
using TodoAPI.Services.Core.Contracts;
using TodoAPI.Services.Core.Exceptions;
using TodoAPI.Services.Core.Models;
using TodoAPI.Services.Foundation.Services;
using Xunit;

namespace TodoAPI.Services.Foundation.Tests.Services
{
    public class TodosServiceTests
    {
        [Fact]
        public async Task GetAllTodosAsync_MakesCorrectCallsAndReturnsCorrectObject()
        {
            // Arrange
            var expectedTodoDTOs = new List<TodoDTO>() { new TodoDTO() { Id = 1, Name = "Test Todo", IsComplete = true } };
            var mockedRepository = new Mock<ITodosRepository>();
            var mockedMapper = new Mock<IMapper>();

            mockedRepository.Setup(r => r.GetAllTodosAsync()).ReturnsAsync(It.IsAny<IEnumerable<Todo>>());
            mockedMapper.Setup(m => m.Map<IEnumerable<Todo>, IEnumerable<TodoDTO>>(It.IsAny<IEnumerable<Todo>>())).Returns(expectedTodoDTOs);

            var service = new TodosService(mockedMapper.Object, mockedRepository.Object);

            // Action
            var actualTodoDTOs = await service.GetAllTodosAsync();

            // Assert
            Assert.NotNull(actualTodoDTOs);
            Assert.Single(actualTodoDTOs);
            Assert.Equal(expectedTodoDTOs, actualTodoDTOs);
            Assert.Equal(expectedTodoDTOs[0].Id, actualTodoDTOs.ToList()[0].Id);
            Assert.Equal(expectedTodoDTOs[0].Name, actualTodoDTOs.ToList()[0].Name);
            Assert.Equal(expectedTodoDTOs[0].IsComplete, actualTodoDTOs.ToList()[0].IsComplete);

            mockedRepository.Verify(r => r.GetAllTodosAsync(), Times.Once());
            mockedMapper.Verify(m => m.Map<IEnumerable<Todo>, IEnumerable<TodoDTO>>(It.IsAny<IEnumerable<Todo>>()), Times.Once());
        }

        [Fact]
        public async Task FindByIdAsync_MakesCorrectCallsAndReturnsCorrectObject()
        {
            // Arrange
            var expectedPrimaryKeyId = 1;
            var expectedTodoDTO = new TodoDTO() { Id = expectedPrimaryKeyId, Name = "Test Todo", IsComplete = true };
            var todo = new Todo();
            var mockedRepository = new Mock<ITodosRepository>();
            var mockedMapper = new Mock<IMapper>();

            mockedRepository.Setup(r => r.FindByIdAsync(expectedPrimaryKeyId)).ReturnsAsync(todo);
            mockedMapper.Setup(m => m.Map<Todo, TodoDTO>(todo)).Returns(expectedTodoDTO);

            var service = new TodosService(mockedMapper.Object, mockedRepository.Object);

            // Action
            var actualTodoDTO = await service.FindByIdAsync(expectedPrimaryKeyId);

            // Assert
            Assert.NotNull(actualTodoDTO);
            Assert.Equal(expectedTodoDTO, actualTodoDTO);

            mockedRepository.Verify(r => r.FindByIdAsync(expectedPrimaryKeyId), Times.Once());
            mockedMapper.Verify(m => m.Map<Todo, TodoDTO>(todo), Times.Once());
        }

        [Fact]
        public async Task CreateTodoAsync_MakesCorrectCallsAndReturnsUpdatedTodoWithId()
        {
            // Arrange
            var expectedCreatedTodoDTO = new CreatedTodoDTO() { Name = "Test Todo", IsComplete = true };
            var expectedTodoDTO = new TodoDTO() { Name = expectedCreatedTodoDTO.Name, IsComplete = expectedCreatedTodoDTO.IsComplete.Value };
            var expectedPrimaryKeyId = 1;
            var todo = new Todo();
            var mockedMapper = new Mock<IMapper>();
            var mockedRepository = new Mock<ITodosRepository>();

            mockedMapper.Setup(m => m.Map<CreatedTodoDTO, Todo>(expectedCreatedTodoDTO)).Returns(todo);
            mockedMapper.Setup(m => m.Map<CreatedTodoDTO, TodoDTO>(expectedCreatedTodoDTO)).Returns(expectedTodoDTO);
            mockedRepository.Setup(r => r.CreateTodoAsync(todo)).ReturnsAsync(expectedPrimaryKeyId);

            var service = new TodosService(mockedMapper.Object, mockedRepository.Object);

            // Action
            var actualTodoDTO = await service.CreateTodoAsync(expectedCreatedTodoDTO);

            // Assert
            Assert.NotNull(actualTodoDTO);
            Assert.Equal(expectedPrimaryKeyId, actualTodoDTO.Id);
            Assert.Equal(expectedTodoDTO.Name, actualTodoDTO.Name);
            Assert.Equal(expectedTodoDTO.IsComplete, actualTodoDTO.IsComplete);

            mockedMapper.Verify(m => m.Map<CreatedTodoDTO, Todo>(expectedCreatedTodoDTO), Times.Once());
            mockedMapper.Verify(m => m.Map<CreatedTodoDTO, TodoDTO>(expectedCreatedTodoDTO), Times.Once());
            mockedRepository.Verify(r => r.CreateTodoAsync(todo), Times.Once());
        }

        [Fact]
        public async Task DeleteTodoByIdAsync_MakesCorrectCalls()
        {
            // Arrange
            var expectedPrimaryKeyId = 1;
            var mockedMapper = new Mock<IMapper>();
            var mockedRepository = new Mock<ITodosRepository>();

            mockedRepository.Setup(r => r.DeleteTodoByIdAsync(expectedPrimaryKeyId));

            var service = new TodosService(mockedMapper.Object, mockedRepository.Object);

            // Action
            await service.DeleteTodoByIdAsync(expectedPrimaryKeyId);

            // Assert
            mockedRepository.Verify(r => r.DeleteTodoByIdAsync(expectedPrimaryKeyId), Times.Once());
        }

        [Fact]
        public async Task UpdateTodoAsync_RecordNotFoundExceptionThrown_ThrowsTodoNotFoundException()
        {
            // Arrange
            var todoDTO = new UpdatedTodoDTO();
            var mockedMapper = new Mock<IMapper>();
            var mockedRepository = new Mock<ITodosRepository>();

            mockedRepository.Setup(r => r.UpdateTodoAsync(It.IsAny<Todo>())).Throws<RecordNotFoundException>();

            var service = new TodosService(mockedMapper.Object, mockedRepository.Object);

            // Action and Assert
            await Assert.ThrowsAsync<TodoNotFoundException>(async () => { await service.UpdateTodoAsync(todoDTO); });
        }

        [Fact]
        public async Task UpdateTodoAsync_ConcurrentAccessExceptionAndItemStillExists_RethrowsConcurrentAccessException()
        {
            // Arrange
            var todoDTO = new UpdatedTodoDTO() { Id = 1 };
            var mockedMapper = new Mock<IMapper>();
            var mockedRepository = new Mock<ITodosRepository>();

            mockedRepository.Setup(r => r.UpdateTodoAsync(It.IsAny<Todo>())).Throws<ConcurrentAccessException>();
            mockedRepository.Setup(r => r.Exists(It.IsAny<long>())).ReturnsAsync(true);

            var service = new TodosService(mockedMapper.Object, mockedRepository.Object);

            // Action and Assert
            await Assert.ThrowsAsync<ConcurrentAccessException>(async () => { await service.UpdateTodoAsync(todoDTO); });
        }

        [Fact]
        public async Task UpdateTodoAsync_ConcurrentAccessExceptionAndItemDoesNotExist_ThrowsTodoNotFoundException()
        {
            // Arrange
            var todoDTO = new UpdatedTodoDTO() { Id = 1 };
            var mockedMapper = new Mock<IMapper>();
            var mockedRepository = new Mock<ITodosRepository>();

            mockedRepository.Setup(r => r.UpdateTodoAsync(It.IsAny<Todo>())).Throws<ConcurrentAccessException>();
            mockedRepository.Setup(r => r.Exists(It.IsAny<long>())).ReturnsAsync(false);

            var service = new TodosService(mockedMapper.Object, mockedRepository.Object);

            // Action and Assert
            await Assert.ThrowsAsync<TodoNotFoundException>(async () => { await service.UpdateTodoAsync(todoDTO); });
        }

        [Fact]
        public async Task UpdateTodoAsync_MakesCorrectCalls()
        {
            // Arrange
            var todoDTO = new UpdatedTodoDTO() { Id = 1, Name = "Updated Todo", IsComplete = true };
            var todo = new Todo() { Id = 1, Name = "Updated Todo", IsComplete = true };
            var mockedMapper = new Mock<IMapper>();
            var mockedRepository = new Mock<ITodosRepository>();

            mockedMapper.Setup(m => m.Map<UpdatedTodoDTO, Todo>(todoDTO)).Returns(todo);
            mockedRepository.Setup(r => r.UpdateTodoAsync(todo));

            var service = new TodosService(mockedMapper.Object, mockedRepository.Object);

            // Action
            await service.UpdateTodoAsync(todoDTO);

            // Assert
            mockedMapper.Verify(m => m.Map<UpdatedTodoDTO, Todo>(todoDTO), Times.Once());
            mockedRepository.Verify(r => r.UpdateTodoAsync(todo), Times.Once());
        }
    }
}
