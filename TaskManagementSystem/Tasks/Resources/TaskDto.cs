
namespace TaskManagementSystem.Tasks.Resources
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Models.TaskStatus Status { get; set; }
        public string? AssignedTo {  get; set; }
    }
}
