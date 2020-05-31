using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Services.Core.Models
{
    public class CreatedTodoDTO
    {
        [Required(ErrorMessage = "The Name field of the Todo is required.")]
        [StringLength(255, ErrorMessage = "The Name field must be between 1 and 255 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The IsComplete field of the Todo is required.")]
        public bool? IsComplete { get; set; }
    }
}
