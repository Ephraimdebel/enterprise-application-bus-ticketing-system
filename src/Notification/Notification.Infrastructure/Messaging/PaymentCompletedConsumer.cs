// using RabbitMQ.Client;
// using RabbitMQ.Client.Events;
// using System.Text;
// using System.Text.Json;
// using Microsoft.Extensions.Hosting;


// namespace Notification.Infrastructure.Messaging
// {
//     public class PaymentCompletedConsumer : BackgroundService
//     {
//         private readonly IConnection _connection;

//         public PaymentCompletedConsumer(IConnection connection)
//         {
//             _connection = connection;
//         }

//         protected override Task ExecuteAsync(CancellationToken stoppingToken)
//         {
//             var channel = _connection.CreateModel();

//             channel.ExchangeDeclare("payment.exchange", ExchangeType.Direct, durable: true);

//             channel.QueueDeclare(
//                 queue: "notification.payment",
//                 durable: true,
//                 exclusive: false,
//                 autoDelete: false);

//             channel.QueueBind("notification.payment", "payment.exchange", "payment.completed");

//             var consumer = new EventingBasicConsumer(channel);

//             consumer.Received += (sender, args) =>
//             {
//                 var json = Encoding.UTF8.GetString(args.Body.ToArray());

//                 Console.WriteLine("📩 Payment Completed Event Received");
//                 Console.WriteLine(json);

//                 // TODO:
//                 // deserialize + store notification + email logic
//             };

//             channel.BasicConsume("notification.payment", true, consumer);

//             return Task.CompletedTask;
//         }
//     }
// }

// Notification.Infrastructure/Messaging/PaymentCompletedConsumer.cs
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notification.Domain;
using Notification.Domain.Entities;
using Notification.Infrastructure;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Notification.Infrastructure.Messaging;

public class PaymentCompletedConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IServiceScopeFactory _scopeFactory;

    public PaymentCompletedConsumer(IConnection connection, IServiceScopeFactory scopeFactory)
    {
        _connection = connection;
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = _connection.CreateModel();
        channel.ExchangeDeclare("payment.exchange", ExchangeType.Direct, durable: true);
        channel.QueueDeclare("payment.completed.queue", durable: true, exclusive: false, autoDelete: false);
        channel.QueueBind("payment.completed.queue", "payment.exchange", "payment.completed");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"[Notification] Received PaymentCompletedEvent: {message}");

    try
    {
        var payment = JsonSerializer.Deserialize<PaymentCompletedEvent>(message);

        if (payment != null)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

            var notification = new NotificationEntity
            {
                Id = Guid.NewGuid(),
                UserId = payment.BookingId,      // TEMP — replace with real user later
                Message = $"Payment of {payment.Amount} {payment.Currency} completed successfully.",
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            await repo.AddAsync(notification);
            Console.WriteLine("[Notification] Saved successfully.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Notification] Error saving notification: {ex.Message}");
    }
};

        channel.BasicConsume(queue: "payment.completed.queue", autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }
}
