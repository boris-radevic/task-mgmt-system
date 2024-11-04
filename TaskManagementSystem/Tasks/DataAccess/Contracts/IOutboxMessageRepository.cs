using TaskManagementSystem.Tasks.Models;

namespace TaskManagementSystem.Tasks.DataAccess.Contracts
{
    public interface IOutboxMessageRepository
    {
        Task<OutboxMessage> AddOutboxMessage(OutboxMessage message);

        IEnumerable<OutboxMessage> GetUnprocessedMessages(int batchSize, string messageType);

        System.Threading.Tasks.Task SetProcessed(IEnumerable<Guid> messages);
    }
}