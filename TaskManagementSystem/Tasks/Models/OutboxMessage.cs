namespace TaskManagementSystem.Tasks.Models
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public string MessageType { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
        public bool Processed { get; set; }
    }
}
