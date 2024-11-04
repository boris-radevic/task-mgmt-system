using TaskManagementSystem.Shared;
using TaskManagementSystem.Tasks.DataAccess.Contracts;

namespace TaskManagementSystem.Tasks.DataAccess.Rabbitmq
{
    public class UpdateTaskProducer : BackgroundService
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IUnitOfWork _unitOfWork;

        private const string routingKey = "TaskUpdated";

        public UpdateTaskProducer(IUnitOfWork unitOfWork, IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
            _unitOfWork = unitOfWork;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();
            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var messages = _unitOfWork.OutboxMessageRepository.GetUnprocessedMessages(10, routingKey);

                    if (messages.Count() > 0)
                    {
                        _eventPublisher.Publish(messages, routingKey);

                        await _unitOfWork.OutboxMessageRepository.SetProcessed(messages.Select(m => m.Id).ToList());
                        Console.WriteLine("Produced a message");
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}