using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Booking.Application;
using global::Booking.Domain;
using Newtonsoft.Json;
using System.Text;

namespace Booking.Infrastructure;

internal sealed class MessagingService
{
    private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

    public MessagingService(Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : IDomainEvent
    {
        await PublishInternalAsync(message, string.Empty, "booking_events", true, cancellationToken);
    }

    public async Task PublishIntegrationEventAsync<T>(T message, string exchange, string routingKey, CancellationToken cancellationToken = default)
    {
        await PublishInternalAsync(message, exchange, routingKey, false, cancellationToken);
    }

    private async Task PublishInternalAsync<T>(T message, string exchange, string routingKey, bool useTypeNameHandling, CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
            Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
            UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest"
        };

        using var connection = await factory.CreateConnectionAsync(cancellationToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        if (!string.IsNullOrEmpty(exchange))
        {
            await channel.ExchangeDeclareAsync(exchange, ExchangeType.Direct, durable: true, cancellationToken: cancellationToken);
        }
        else
        {
            await channel.QueueDeclareAsync(
                queue: routingKey,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);
        }

        var settings = new JsonSerializerSettings();
        if (useTypeNameHandling)
        {
            settings.TypeNameHandling = TypeNameHandling.All;
        }
        else
        {
            // For integration events, we use camelCase usually but let's stick to what matches the DTOs
            // settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        var json = JsonConvert.SerializeObject(message, settings);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: exchange,
            routingKey: routingKey,
            body: body,
            cancellationToken: cancellationToken);
    }
}
