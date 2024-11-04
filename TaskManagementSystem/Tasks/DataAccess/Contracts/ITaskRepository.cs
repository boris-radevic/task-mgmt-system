namespace TaskManagementSystem.Tasks.DataAccess.Contracts
{
    public interface ITaskRepository
    {
        Task<Models.Task?> AddTask(Models.Task task);
        Task UpdateTaskStatus(int taskId, int newStatusValue);
        Task<ICollection<Models.Task>> GetAllTasks();
    }
}
