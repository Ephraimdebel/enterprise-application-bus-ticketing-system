using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notification.Domain;
using Notification.Domain.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Notification.Infrastructure.Messaging;

public class BookingConfirmedConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IServiceScopeFactory _scopeFactory;

    public BookingConfirmedConsumer(
        IConnection connection,
        IServiceScopeFactory scopeFactory)
    {
        _connection = connection;
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = _connection.CreateModel();

        channel.ExchangeDeclare("booking.exchange", ExchangeType.Direct, durable: true);
        channel.QueueDeclare(
            queue: "booking.confirmed.queue",
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        channel.QueueBind(
            queue: "booking.confirmed.queue",
            exchange: "booking.exchange",
            routingKey: "booking.confirmed"
        );

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"[Notification] Received BookingConfirmedEvent: {message}");

            try
            {
                var booking =
                    JsonSerializer.Deserialize<BookingConfirmedForNotificationEvent>(message);

                if (booking != null)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var repo = scope.ServiceProvider
                        .GetRequiredService<INotificationRepository>();

                    var notification = new NotificationEntity
                    {
                        Id = Guid.NewGuid(),
                        UserId = booking.PassengerId,
                        Message = $"Your booking {booking.BookingId} has been confirmed.",
                        CreatedAt = DateTime.UtcNow,
                        IsRead = false
                    };

                    await repo.AddAsync(notification);
                    Console.WriteLine("[Notification] Booking notification saved.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Notification] Error: {ex.Message}");
            }
        };

        channel.BasicConsume(
            queue: "booking.confirmed.queue",
            autoAck: true,
            consumer: consumer
        );

        return Task.CompletedTask;
    }
}
