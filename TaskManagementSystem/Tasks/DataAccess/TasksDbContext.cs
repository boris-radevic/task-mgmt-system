using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Tasks.Models;

namespace TaskManagementSystem.Tasks.DataAccess
{
    public class TasksDbContext : DbContext
    {
        public TasksDbContext()
        {
        }

        public TasksDbContext(DbContextOptions<TasksDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new TasksDbConfiguration());
        }

        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
    }
}