using System.Text;
using Dispute.Application;
using Dispute.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Dispute.Infrastructure;

internal sealed class EventSubscriberService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EventSubscriberService> _logger;
    private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

    public EventSubscriberService(
        IServiceProvider serviceProvider,
        ILogger<EventSubscriberService> logger,
        Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
            Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
            UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest"
        };

        using var connection = await factory.CreateConnectionAsync(stoppingToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(
            queue: "booking_events",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            
            _logger.LogInformation("Received event: {Message}", message);

            try
            {
                // Simple integration logic: if booking is cancelled, we might want to log it or prepare for a potential dispute
                // In a real scenario, we would deserialize to a specific IntegrationEvent type
                if (message.Contains("BookingCancelledDomainEvent"))
                {
                    await HandleBookingCancelledAsync(message, stoppingToken);
                }
                else if (message.Contains("PaymentFailedDomainEvent"))
                {
                    await HandlePaymentFailedAsync(message, stoppingToken);
                }
                else if (message.Contains("BookingFailedDomainEvent"))
                {
                    await HandleBookingFailedAsync(message, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing event");
            }

            await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
        };

        await channel.BasicConsumeAsync(
            queue: "booking_events",
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        _logger.LogInformation("EventSubscriberService started listening to booking_events");

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task HandleBookingFailedAsync(string message, CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<MediatR.ISender>();

        _logger.LogInformation("Processing Booking Failure in Dispute Module...");

        try
        {
            var data = JsonConvert.DeserializeObject<dynamic>(message);
            if (data == null) return;

            Guid bookingId = data.BookingId;
            Guid passengerId = data.PassengerId;

            var command = new OpenDisputeCommand(
                bookingId,
                passengerId,
                "AUTO_FAIL",
                "Automatic dispute for failed booking.",
                "System: Booking failed. Opening dispute for investigation.");

            await sender.Send(command, ct);
            _logger.LogInformation("Automatically opened dispute for Booking: {BookingId}", bookingId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to automatically open dispute");
        }
    }

    private async Task HandleBookingCancelledAsync(string message, CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        // Here we could automatically open a dispute or notify support
        _logger.LogInformation("Processing Booking Cancellation in Dispute Module...");
    }

    private async Task HandlePaymentFailedAsync(string message, CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        _logger.LogInformation("Processing Payment Failure in Dispute Module...");
    }
}
