namespace TaskManagementSystem.Shared
{
    public interface IEventPublisher
    {
        void Publish<T>(T eventMessage, string routingKey);
    }
}