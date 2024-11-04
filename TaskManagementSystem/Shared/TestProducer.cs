using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace TaskManagementSystem.Shared
{
    public class TestProducer : IEventPublisher
    {
        private readonly IModel _model;
        private readonly ILogger logger;

        public TestProducer(RabbitMqService connection)
        {
            _model = connection.CreateChannel();
            _model.ExchangeDeclare("exchange", ExchangeType.Topic);
        }

        public void Publish<T>(T eventMessage, string routingKey)
        {
            var message = JsonSerializer.Serialize(eventMessage);
            var body = Encoding.UTF8.GetBytes(message);

            IBasicProperties properties = _model.CreateBasicProperties();

            _model.BasicPublish("exchange", routingKey, basicProperties: null, body: body);
        }
    }
}