namespace TaskManagementSystem.Shared
{
    public class RabbitMqConfiguration
    {
        public static readonly string keyName = "App:RabbitmqConfig";
        public string HostName { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}