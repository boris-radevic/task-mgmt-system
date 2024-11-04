using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TaskManagementSystem.Exceptions;
using TaskManagementSystem.Shared;
using TaskManagementSystem.Tasks.DataAccess.Contracts;
using TaskManagementSystem.Tasks.Resources;

namespace TaskManagementSystem.Tasks.DataAccess.Rabbitmq
{
    public class DLQRetryConsumer : IEventConsumer
    {
        private readonly IModel _channel;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _producer;

        public DLQRetryConsumer(RabbitMqService service, IUnitOfWork unitOfWork, IEventPublisher producer)
        {
            _channel = service.CreateChannel();
            _unitOfWork = unitOfWork;
            _producer = producer;
        }
        public void StartConsuming()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                try
                {
                    
                    if (message != null)
                    {
                        switch (ea.RoutingKey)
                        {
                            case "Task.Add.Consume":
                                var addEvent = JsonSerializer.Deserialize<Models.Task>(message);
                                _producer.Publish(addEvent, ea.RoutingKey);
                                break;
                            case "Task.Update.Consume":
                                var updateEvent = JsonSerializer.Deserialize<TaskUpdateDto>(message);
                                _producer.Publish(updateEvent, ea.RoutingKey);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        throw new BadRequestException("");
                    }

                    _channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                }


            };

            _channel.BasicConsume(queue: "dead_letter", autoAck: false, consumer: consumer);
        }
    }
}
