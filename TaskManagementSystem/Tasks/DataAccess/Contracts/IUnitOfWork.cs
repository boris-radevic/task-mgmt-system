namespace TaskManagementSystem.Tasks.DataAccess.Contracts
{
    public interface IUnitOfWork
    {
        void Save();
        Task SaveAsync();
        ITaskRepository TaskRepository { get; }
        IOutboxMessageRepository OutboxMessageRepository { get; }
    }
}
