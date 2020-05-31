using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TodoAPI.Services.Core.Models;
using Xunit;

namespace TodoAPI.Tests.ModelState
{
    public class ModelStateValidationTests
    {
        [Fact]
        public void CreatedTodoDTO_NameIsMissing_GeneratesModelStateError()
        {
            // Arrange
            var results = new List<ValidationResult>();
            var createdTodo = new CreatedTodoDTO() { IsComplete = true };

            var isValid = Validator.TryValidateObject(createdTodo, new ValidationContext(createdTodo), results, true);

            Assert.False(isValid);
            Assert.Single(results);
            Assert.Equal("The Name field of the Todo is required.", results[0].ErrorMessage);
        }

        [Fact]
        public void CreatedTodoDTO_IsCompleteIsMissing_GeneratesModelStateError()
        {
            // Arrange
            var results = new List<ValidationResult>();
            var createdTodo = new CreatedTodoDTO() { Name = "Hello World" };

            var isValid = Validator.TryValidateObject(createdTodo, new ValidationContext(createdTodo), results, true);

            Assert.False(isValid);
            Assert.Single(results);
            Assert.Equal("The IsComplete field of the Todo is required.", results[0].ErrorMessage);
        }

        [Fact]
        public void CreatedTodoDTO_NameIsTooLong_GeneratesModelStateError()
        {
            // Arrange
            var name = "".PadLeft(256, '*');
            var results = new List<ValidationResult>();
            var createdTodo = new CreatedTodoDTO() { Name = name, IsComplete = true };

            var isValid = Validator.TryValidateObject(createdTodo, new ValidationContext(createdTodo), results, true);

            Assert.False(isValid);
            Assert.Single(results);
            Assert.Equal("The Name field must be between 1 and 255 characters.", results[0].ErrorMessage);
        }

        [Fact]
        public void CreatedTodoDTO_NameIs255Characters_NoModelStateError()
        {
            // Arrange
            var name = "".PadLeft(255, '*');
            var results = new List<ValidationResult>();
            var createdTodo = new CreatedTodoDTO() { Name = name, IsComplete = true };

            var isValid = Validator.TryValidateObject(createdTodo, new ValidationContext(createdTodo), results, true);

            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void CreatedTodoDTO_NameIsBetween1And255Characters_NoModelStateError()
        {
            // Arrange
            var name = "".PadLeft(100, '*');
            var results = new List<ValidationResult>();
            var createdTodo = new CreatedTodoDTO() { Name = name, IsComplete = true };

            var isValid = Validator.TryValidateObject(createdTodo, new ValidationContext(createdTodo), results, true);

            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void UpdatedTodoDTO_IdIsMissing_GeneratesModelStateError()
        {
            // Arrange
            var results = new List<ValidationResult>();
            var createdTodo = new UpdatedTodoDTO() { Name = "Hello World", IsComplete = true };

            var isValid = Validator.TryValidateObject(createdTodo, new ValidationContext(createdTodo), results, true);

            Assert.False(isValid);
            Assert.Single(results);
            Assert.Equal("The Id field of the Todo is required.", results[0].ErrorMessage);
        }

        [Fact]
        public void UpdatedTodoDTO_NameIsMissing_GeneratesModelStateError()
        {
            // Arrange
            var results = new List<ValidationResult>();
            var createdTodo = new UpdatedTodoDTO() { Id = 1, IsComplete = true };

            var isValid = Validator.TryValidateObject(createdTodo, new ValidationContext(createdTodo), results, true);

            Assert.False(isValid);
            Assert.Single(results);
            Assert.Equal("The Name field of the Todo is required.", results[0].ErrorMessage);
        }

        [Fact]
        public void UpdatedTodoDTO_IsCompleteIsMissing_GeneratesModelStateError()
        {
            // Arrange
            var results = new List<ValidationResult>();
            var createdTodo = new UpdatedTodoDTO() { Id = 1, Name = "Hello World" };

            var isValid = Validator.TryValidateObject(createdTodo, new ValidationContext(createdTodo), results, true);

            Assert.False(isValid);
            Assert.Single(results);
            Assert.Equal("The IsComplete field of the Todo is required.", results[0].ErrorMessage);
        }

        [Fact]
        public void UpdatedTodoDTO_NameIsTooLong_GeneratesModelStateError()
        {
            // Arrange
            var name = "".PadLeft(256, '*');
            var results = new List<ValidationResult>();
            var createdTodo = new UpdatedTodoDTO() { Id = 1, Name = name, IsComplete = true };

            var isValid = Validator.TryValidateObject(createdTodo, new ValidationContext(createdTodo), results, true);

            Assert.False(isValid);
            Assert.Single(results);
            Assert.Equal("The Name field must be between 1 and 255 characters.", results[0].ErrorMessage);
        }

        [Fact]
        public void UpdatedTodoDTO_NameIs255Characters_NoModelStateError()
        {
            // Arrange
            var name = "".PadLeft(255, '*');
            var results = new List<ValidationResult>();
            var createdTodo = new UpdatedTodoDTO() { Id = 1, Name = name, IsComplete = true };

            var isValid = Validator.TryValidateObject(createdTodo, new ValidationContext(createdTodo), results, true);

            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void UpdatedTodoDTO_NameIsBetween1And255Characters_NoModelStateError()
        {
            // Arrange
            var name = "".PadLeft(100, '*');
            var results = new List<ValidationResult>();
            var createdTodo = new UpdatedTodoDTO() { Id = 1, Name = name, IsComplete = true };

            var isValid = Validator.TryValidateObject(createdTodo, new ValidationContext(createdTodo), results, true);

            Assert.True(isValid);
            Assert.Empty(results);
        }
    }
}
