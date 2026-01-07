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
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
            Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
            UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest"
        };

        using var connection = await factory.CreateConnectionAsync(cancellationToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: "booking_events",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        var json = JsonConvert.SerializeObject(message, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });
        
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: "booking_events",
            body: body,
            cancellationToken: cancellationToken);
    }
}
