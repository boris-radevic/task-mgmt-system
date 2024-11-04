using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace TaskManagementSystem.Shared
{
    public class RabbitMqService
    {
        private readonly RabbitMqConfiguration _rabbitMqConfiguration;
        private readonly ConnectionFactory _factory;
        private const string deadLetterExchange = "dead_letter_exchange";
        private const string mainExchange = "exchange";

        public RabbitMqService(IOptions<RabbitMqConfiguration> config)
        {
            _rabbitMqConfiguration = config.Value;

            _factory = new ConnectionFactory()
            {
                HostName = _rabbitMqConfiguration.HostName,
                Port = _rabbitMqConfiguration.Port,
                UserName = _rabbitMqConfiguration.Username,
                Password = _rabbitMqConfiguration.Password,
            };
            _factory.DispatchConsumersAsync = true;
        }

        public IModel CreateChannel()
        {
            var _connection = _factory.CreateConnection();
            var channel = _connection.CreateModel();

            //Exchanges
            channel.ExchangeDeclare(mainExchange, ExchangeType.Topic);
            channel.ExchangeDeclare(deadLetterExchange, ExchangeType.Fanout);
            channel.QueueDeclare("dead_letter", true, false, false, null);
            channel.QueueBind("dead_letter", deadLetterExchange, "");
            //Queues
            Dictionary<string, object> args = new Dictionary<string, object>()
            {
                { "x-dead-letter-exchange", deadLetterExchange }
            };
            channel.QueueDeclare(queue: "add_task_queue", durable: true, exclusive: false, autoDelete: false, arguments: args);
            channel.QueueBind(queue: "add_task_queue", exchange: mainExchange, routingKey: "Task.Add.Consume");

            channel.QueueDeclare(queue: "update_task_queue", durable: true, exclusive: false, autoDelete: false, arguments: args);
            channel.QueueBind(queue: "update_task_queue", exchange: mainExchange, routingKey: "Task.Update.Consume");

            channel.QueueDeclare(queue: "action-done-queue", durable: true, exclusive: false, autoDelete: false, null);
            channel.QueueBind(queue: "action-done-queue", exchange: mainExchange, routingKey: "Action.Done");

            //var _connection = _factory.CreateConnection();
            //var channel = _connection.CreateModel();

            //channel.ExchangeDeclare(mainExchange, ExchangeType.Topic);
            //Dictionary<string, object> args = new Dictionary<string, object>()
            //{
            //    { "x-dead-letter-exchange", deadLetterExchange },
            //};
            ////UPDATE TASK
            //var argsUpdateQueue = new Dictionary<string, object>();
            //argsUpdateQueue.Add("x-message-ttl", 60000);
            //channel.QueueDeclare(queue: "update_task_queue", durable: true, exclusive: false, autoDelete: false, arguments: args);
            //channel.QueueBind(queue: "update_task_queue", exchange: mainExchange, routingKey: "Task.Update.Consume");

            ////DEAD LETER
            //channel.ExchangeDeclare(deadLetterExchange, ExchangeType.Fanout);
            //channel.QueueDeclare("dead_letter", true, false, false, null);
            //channel.QueueBind("dead_letter", deadLetterExchange, "");

            ////ADD TASK

            //channel.QueueDeclare(queue: "add_task_queue", durable: true, exclusive: false, autoDelete: false, arguments: args);
            //channel.QueueBind(queue: "add_task_queue", exchange: mainExchange, routingKey: "Task.Add.Consume");


            return channel;
        }
    }
}