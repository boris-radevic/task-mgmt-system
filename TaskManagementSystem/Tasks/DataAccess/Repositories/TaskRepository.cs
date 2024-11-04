using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Exceptions;
using TaskManagementSystem.Tasks.DataAccess.Contracts;

namespace TaskManagementSystem.Tasks.DataAccess.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private TasksDbContext _context;

        public TaskRepository(TasksDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Task?> AddTask(Models.Task task)
        {
            var taskEntity = await _context.Tasks.AddAsync(task);
            return taskEntity.Entity;
        }

        public async Task<ICollection<Models.Task>> GetAllTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task UpdateTaskStatus(int taskId, int newStatusValue)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
            {
                throw new NotFoundException("Task not found");
            }
            if (Enum.IsDefined(typeof(Models.TaskStatus), task.Status))
            {
                task.Status = (Models.TaskStatus)newStatusValue;
            }
            else
            {
                throw new ArgumentException("Invalid status value");
            }
        }
    }
}