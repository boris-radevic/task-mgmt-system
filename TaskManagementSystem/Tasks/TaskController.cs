using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Tasks.Resources;

namespace TaskManagementSystem.Tasks
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost("AddTask")]
        public async Task<IActionResult> AddTask(AddTaskDto dto)
        {
            await _taskService.AddTask(dto);
            return Ok();
        }

        [HttpPost("AddTask/Event")]
        public async Task<IActionResult> AddTaskEvent(AddTaskDto dto)
        {
            await _taskService.AddTaskEvent(dto);
            return Ok();
        }

        [HttpPost("UpdateTask/Event")]
        public async Task<IActionResult> UpdateTaskEvent(TaskUpdateDto dto)
        {
            await _taskService.UpdateTaskEvent(dto);
            return Ok();
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> UpdateTaskStatus(TaskUpdateDto dto)
        {
            await _taskService.UpdateTaskStatus(dto);
            return Ok();
        }

        [HttpGet("GetTasks")]
        [Produces("application/json", "application/xml", Type = typeof(ICollection<TaskDto>))]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _taskService.GetTasks();
            return Ok(tasks);
        }
    }
}