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
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class PaymentCompletedConsumer : BackgroundService
{
    private readonly IConnection _connection;

    public PaymentCompletedConsumer(IConnection connection)
    {
        _connection = connection;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = _connection.CreateModel();
        channel.ExchangeDeclare("payment.exchange", ExchangeType.Direct);
        channel.QueueDeclare("payment.completed.queue", durable: true, exclusive: false, autoDelete: false);
        channel.QueueBind("payment.completed.queue", "payment.exchange", "payment.completed");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"[Notification] Received PaymentCompletedEvent: {message}");
            // Here you can save to DB or send email, etc.
        };

        channel.BasicConsume(queue: "payment.completed.queue", autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }
}
