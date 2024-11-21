using ToDoList.Data.Entities;

namespace ToDoList.Models
{
    public class TasksListViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateOnly Date { get; set; }
        public List<TaskViewModel>? Tasks { get; set; } = new List<TaskViewModel>();
    }
}
