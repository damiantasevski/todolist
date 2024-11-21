using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Data.Entities
{
    public class TasksList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateOnly Date { get; set; }

        //external references
        public ICollection<Task>? Tasks { get; set; } = new List<Task>();

        public void OrderTasks()
        {
            if (this.Tasks != null)
            {
                int order = 0;
                foreach (var task in this.Tasks)
                {
                    task.Order = order++;
                }
            }
        }
    }
}
