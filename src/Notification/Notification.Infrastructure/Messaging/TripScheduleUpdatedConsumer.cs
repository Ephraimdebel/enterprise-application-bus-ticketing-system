using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notification.Domain;
using Notification.Domain.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Notification.Infrastructure.Messaging;

public class TripScheduleUpdatedConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IServiceScopeFactory _scopeFactory;

    public TripScheduleUpdatedConsumer(
        IConnection connection,
        IServiceScopeFactory scopeFactory)
    {
        _connection = connection;
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = _connection.CreateModel();

        channel.ExchangeDeclare("trip.exchange", ExchangeType.Fanout, durable: true);
        channel.QueueDeclare(
            queue: "notification.trip.updates",
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        channel.QueueBind(
            queue: "notification.trip.updates",
            exchange: "trip.exchange",
            routingKey: ""
        );

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"[Notification] Received TripScheduleUpdatedEvent: {message}");

            try
            {
                // Simple logic: notify all users affected or just log
                // In a real system, we'd lookup bookings for this trip
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Notification] Error: {ex.Message}");
            }
        };

        channel.BasicConsume(
            queue: "notification.trip.updates",
            autoAck: true,
            consumer: consumer
        );

        return Task.CompletedTask;
    }
}
