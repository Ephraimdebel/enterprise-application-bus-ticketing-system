using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Booking.Application;
using MediatR;

namespace Booking.Infrastructure.Messaging;

internal sealed class PaymentCompletedConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PaymentCompletedConsumer> _logger;
    private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

    public PaymentCompletedConsumer(
        IServiceProvider serviceProvider,
        ILogger<PaymentCompletedConsumer> logger,
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

        await channel.ExchangeDeclareAsync(
            exchange: "payment.exchange",
            type: ExchangeType.Direct,
            durable: true,
            cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(
            queue: "payment.completed.queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        await channel.QueueBindAsync(
            queue: "payment.completed.queue",
            exchange: "payment.exchange",
            routingKey: "payment.completed",
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            
            _logger.LogInformation("Received PaymentCompletedEvent: {Message}", message);

            try
            {
                var paymentEvent = JsonConvert.DeserializeObject<dynamic>(message);
                if (paymentEvent != null)
                {
                    Guid bookingId = paymentEvent.BookingId;
                    
                    using var scope = _serviceProvider.CreateScope();
                    var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                    
                    await sender.Send(new CompleteBookingCommand(bookingId), stoppingToken);
                    
                    _logger.LogInformation("Successfully completed booking for BookingId: {BookingId}", bookingId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing PaymentCompletedEvent");
            }

            await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
        };

        await channel.BasicConsumeAsync(
            queue: "payment.completed.queue",
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        _logger.LogInformation("PaymentCompletedConsumer started listening");

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
