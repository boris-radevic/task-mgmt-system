using Mapster;
using Newtonsoft.Json;
using TaskManagementSystem.Shared;
using TaskManagementSystem.Tasks.DataAccess.Contracts;
using TaskManagementSystem.Tasks.Models;
using TaskManagementSystem.Tasks.Resources;

namespace TaskManagementSystem.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _producer;

        public TaskService(IUnitOfWork unitOfWork, IEventPublisher producer)
        {
            _unitOfWork = unitOfWork;
            _producer = producer;
        }

        public async System.Threading.Tasks.Task AddTask(AddTaskDto taskDto)
        {
            var newTask = taskDto.Adapt<Models.Task>();
            await _unitOfWork.TaskRepository.AddTask(newTask);

            var newOutboxMessage = new OutboxMessage
            {
                MessageType = "TaskCreated",
                Payload = JsonConvert.SerializeObject(newTask),
                CreatedAt = DateTimeOffset.Now,
                Processed = false
            };
            await _unitOfWork.OutboxMessageRepository.AddOutboxMessage(newOutboxMessage);
            await _unitOfWork.SaveAsync();
        }

        public async System.Threading.Tasks.Task AddTaskEvent(AddTaskDto taskDto)
        {
            var newTask = taskDto.Adapt<Models.Task>();
            _producer.Publish(taskDto, "Task.Add.Consume");
        }

        public async System.Threading.Tasks.Task UpdateTaskEvent(TaskUpdateDto updateDto)
        {
            _producer.Publish(updateDto, "Task.Update.Consume");
        }

        public async System.Threading.Tasks.Task<List<TaskDto>> GetTasks()
        {
            var tasks = await _unitOfWork.TaskRepository.GetAllTasks();
            return tasks.Adapt<ICollection<TaskDto>>().ToList();
        }

        public async System.Threading.Tasks.Task UpdateTaskStatus(TaskUpdateDto dto)
        {
            await _unitOfWork.TaskRepository.UpdateTaskStatus(dto.TaskId, dto.NewStatusId);
            var newOutboxMessage = new OutboxMessage
            {
                MessageType = "TaskUpdated",
                Payload = JsonConvert.SerializeObject(dto),
                CreatedAt = DateTimeOffset.Now,
                Processed = false
            };
            await _unitOfWork.OutboxMessageRepository.AddOutboxMessage(newOutboxMessage);
            await _unitOfWork.SaveAsync();
        }
    }
}