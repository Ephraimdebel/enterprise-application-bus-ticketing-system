// using System.Text;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Options;
// using RabbitMQ.Client;

// namespace Passenger.Infrastructure.Outbox;

// public sealed class RabbitMqPublisher : IRabbitMqPublisher, IDisposable
// {
//     private readonly RabbitMqOptions _options;
//     private readonly ILogger<RabbitMqPublisher> _logger;
//     private readonly IConnection _connection;
//     private readonly IModel _channel;

//     public RabbitMqPublisher(IOptions<RabbitMqOptions> options, ILogger<RabbitMqPublisher> logger)
//     {
//         _options = options.Value;
//         _logger = logger;

//         var factory = new ConnectionFactory
//         {
//             HostName = _options.HostName,
//             Port = _options.Port,
//             UserName = _options.UserName,
//             Password = _options.Password,
//             VirtualHost = _options.VirtualHost,
//             DispatchConsumersAsync = true
//         };

//         _connection = factory.CreateConnection();
//         _channel = _connection.CreateModel();

//         _channel.ExchangeDeclare(exchange: _options.ExchangeName, type: ExchangeType.Topic, durable: true, autoDelete: false);

//         _logger.LogInformation("RabbitMQ publisher connected to {Host}:{Port}, exchange={Exchange}",
//             _options.HostName, _options.Port, _options.ExchangeName);
//     }

//     public Task PublishAsync(string eventType, string jsonPayload, CancellationToken cancellationToken = default)
//     {
//         // routing key: use event type name
//         var routingKey = eventType.Split('.').LastOrDefault() ?? eventType;

//         var body = Encoding.UTF8.GetBytes(jsonPayload);

//         var props = _channel.CreateBasicProperties();
//         props.ContentType = "application/json; charset=utf-8";
//         props.DeliveryMode = 2; // persistent

//         _channel.BasicPublish(
//             exchange: _options.ExchangeName,
//             routingKey: routingKey,
//             basicProperties: props,
//             body: body);

//         return Task.CompletedTask;
//     }

//     public void Dispose()
//     {
//         try { _channel.Close(); } catch { }
//         try { _connection.Close(); } catch { }
//     }
// }
