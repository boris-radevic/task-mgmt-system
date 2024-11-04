namespace TaskManagementSystem.Tasks.Resources
{
    public class ActionDoneDto
    {
        public string MessageType { get; set; } = String.Empty;
        public int TaskId { get; set; }
        public DateTimeOffset FinishedAt { get; set; }
    }
}
