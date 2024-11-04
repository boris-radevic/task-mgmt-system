using TaskManagementSystem.Tasks.Resources;

namespace TaskManagementSystem.Tasks
{
    public interface ITaskService
    {
        Task AddTask(AddTaskDto taskDto);

        Task AddTaskEvent(AddTaskDto taskDto);

        Task UpdateTaskEvent(TaskUpdateDto updateDto);

        Task UpdateTaskStatus(TaskUpdateDto dto);

        Task<List<TaskDto>> GetTasks();
    }
}