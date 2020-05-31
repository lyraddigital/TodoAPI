namespace TodoAPI.Services.Core.Models
{
    public class TodoDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
