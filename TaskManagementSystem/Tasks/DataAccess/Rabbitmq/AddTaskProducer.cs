using TaskManagementSystem.Shared;
using TaskManagementSystem.Tasks.DataAccess.Contracts;

namespace TaskManagementSystem.Tasks.DataAccess.Rabbitmq
{
    public class AddTaskProducer : BackgroundService
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IUnitOfWork _unitOfWork;
        private const string routingKey = "TaskCreated";

        public AddTaskProducer(IEventPublisher eventPublisher, IUnitOfWork unitOfWork)
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
            while (!stoppingToken.IsCancellationRequested)
            {
                var messages = _unitOfWork.OutboxMessageRepository.GetUnprocessedMessages(10, routingKey);

                if (messages.Count() > 0)
                {
                    _eventPublisher.Publish(messages.Select(m => m.Payload).ToList(), routingKey);

                    await _unitOfWork.OutboxMessageRepository.SetProcessed(messages.Select(m => m.Id).ToList());
                    Console.WriteLine("Produced a message");
                }
                else
                {
                }
            }
        }
    }
}