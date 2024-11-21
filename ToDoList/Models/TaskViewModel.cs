using Task = ToDoList.Data.Entities.Task;

namespace ToDoList.Models
{
    public class TaskViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsDone { get; set; }
        public int Order { get; set; }
    }
}
