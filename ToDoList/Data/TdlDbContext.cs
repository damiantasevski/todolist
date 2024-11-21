using Microsoft.EntityFrameworkCore;
using ToDoList.Data.Entities;
using Task = ToDoList.Data.Entities.Task;

namespace ToDoList.Data
{
    public class TdlDbContext : DbContext
    {
        public DbSet<TasksList> TasksLists { get; set; }
        public DbSet<Task> Tasks { get; set; }

        public TdlDbContext(DbContextOptions<TdlDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
