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
    public class AddTaskConsumer : IEventConsumer
    {
        private readonly IModel _channel;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _producer;

        public AddTaskConsumer(RabbitMqService connection, IUnitOfWork unitOfWork, IEventPublisher producer)
        {
            _channel = connection.CreateChannel();
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
                var addEvent = JsonSerializer.Deserialize<Models.Task>(message);

                try
                {
                    
                    if (addEvent != null)
                    {
                        await _unitOfWork.TaskRepository.AddTask(addEvent);
                        await _unitOfWork.SaveAsync();

                        var actionDone = new ActionDoneDto
                        {
                            MessageType = "TaskCreated",
                            TaskId = addEvent.Id,
                            FinishedAt = DateTimeOffset.Now
                        };
                        _channel.BasicAck(ea.DeliveryTag, multiple: false);

                        _producer.Publish(actionDone, "Action.Done");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, false, requeue: false);
                }


            };
            _channel.BasicConsume(queue: "add_task_queue", autoAck: false, consumer: consumer);
        }
    }
}