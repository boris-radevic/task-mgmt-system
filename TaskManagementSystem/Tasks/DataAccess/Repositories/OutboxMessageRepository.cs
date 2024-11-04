using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Tasks.DataAccess.Contracts;
using TaskManagementSystem.Tasks.Models;

namespace TaskManagementSystem.Tasks.DataAccess.Repositories
{
    public class OutboxMessageRepository : IOutboxMessageRepository
    {
        private TasksDbContext _context;

        public OutboxMessageRepository(TasksDbContext context)
        {
            _context = context;
        }

        public async Task<OutboxMessage> AddOutboxMessage(OutboxMessage message)
        {
            var outboxMessageEntity = await _context.OutboxMessages.AddAsync(message);
            return outboxMessageEntity.Entity;
        }

        public async System.Threading.Tasks.Task SetProcessed(IEnumerable<Guid> messages)
        {
            await _context.OutboxMessages.Where(m => messages.Contains(m.Id)).ExecuteUpdateAsync(m => m.SetProperty(x => x.Processed, x => true));
        }

        public IEnumerable<OutboxMessage> GetUnprocessedMessages(int batchSize, string messageType)
        {
            var outboxMessages = _context.OutboxMessages.Where(m => !m.Processed)
                .Take(batchSize).ToList();

            return outboxMessages;
        }
    }
}