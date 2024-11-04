using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using TaskManagementSystem.Tasks.DataAccess.Contracts;

namespace TaskManagementSystem.Tasks.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TasksDbContext _context;
        private ITaskRepository _taskRepository = null!;
        private IOutboxMessageRepository _outboxMessageRepository = null!;

        public UnitOfWork(IDbContextFactory<TasksDbContext> context)
        {
            _context = context.CreateDbContext();
        }

        public ITaskRepository TaskRepository
        {
            get
            {
                if (_taskRepository == null)
                {
                    _taskRepository = new TaskRepository(_context);
                }
                return _taskRepository;
            }
        }

        public IOutboxMessageRepository OutboxMessageRepository
        {
            get
            {
                if (_outboxMessageRepository == null)
                {
                    _outboxMessageRepository = new OutboxMessageRepository(_context);
                }
                return _outboxMessageRepository;
            }
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException dex)
            {
                string message = "Data is not saved.";
                foreach (var entry in dex.Entries)
                {
                    message += "\n" + entry.DebugView.ShortView;
                }
                throw new Exception(message, dex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dex)
            {
                string message = "Data is not saved.";
                foreach (var entry in dex.Entries)
                {
                    message += "\n" + entry.DebugView.ShortView;
                }
                throw new Exception(message, dex);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}