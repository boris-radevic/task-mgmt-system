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
    public class UpdateTaskConsumer : IEventConsumer
    {
        private readonly IModel _channel;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _producer;


        public UpdateTaskConsumer(RabbitMqService service, IUnitOfWork unitOfWork, IEventPublisher producer)
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
                var updateEvent = JsonSerializer.Deserialize<TaskUpdateDto>(message);

                try
                {
                    if (updateEvent != null)
                    {
                        await _unitOfWork.TaskRepository.UpdateTaskStatus(updateEvent.TaskId, updateEvent.NewStatusId);
                        await _unitOfWork.SaveAsync();

                        var actionDone = new ActionDoneDto
                        {
                            MessageType = "TaskUpdated",
                            TaskId = updateEvent.TaskId,
                            FinishedAt = DateTimeOffset.Now
                        };

                        _channel.BasicAck(ea.DeliveryTag, multiple: false);
                        _producer.Publish(actionDone, "Action.Done");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                }

                
            };

            _channel.BasicConsume(queue: "update_task_queue", autoAck: false, consumer: consumer);
        }
    }
}