using TaskManagementSystem.Shared;

namespace TaskManagementSystem.Tasks.DataAccess.Rabbitmq
{
    public class ConsumerHostedService : BackgroundService
    {
        private readonly IEnumerable<IEventConsumer> _consumers;

        public ConsumerHostedService(IEnumerable<IEventConsumer> consumers)
        {
            _consumers = consumers;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IList<Task> tasks = new List<Task>();

            foreach (var consumer in _consumers)
            {
                tasks.Add(Task.Run(() =>
                {
                    consumer.StartConsuming();
                }));
            }

            await Task.WhenAll(tasks);
        }
    }
}