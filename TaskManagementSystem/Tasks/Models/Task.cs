namespace TaskManagementSystem.Tasks.Models
{
    public enum TaskStatus
    {
        NotStarted,
        InProgress,
        Completed
    }

    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskStatus Status { get; set; }
        public string? AssignedTo { get; set; }
    }
}
