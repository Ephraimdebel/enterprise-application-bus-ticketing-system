// using RabbitMQ.Client;
// using System.Text;
// using System.Text.Json;

// namespace Payment.Infrastructure.Messaging
// {
//     public class RabbitMQPublisher
//     {
//         private readonly IConnection _connection;

//         public RabbitMQPublisher(IConnection connection)
//         {
//             _connection = connection;
//         }

//         public void Publish(object message, string exchange, string routingKey)
//         {
//             using var channel = _connection.CreateModel();

//             channel.ExchangeDeclare(exchange, ExchangeType.Direct, durable: true);

//             var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

//             channel.BasicPublish(
//                 exchange: exchange,
//                 routingKey: routingKey,
//                 basicProperties: null,
//                 body: body
//             );
//         }
//     }
// }


// Payment.Infrastructure/Messaging/RabbitMQPublisher.cs
using Payment.Application.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Payment.Infrastructure.Messaging
{
    public class RabbitMQPublisher : IEventPublisher
    {
        private readonly IConnection _connection;

        public RabbitMQPublisher(IConnection connection)
        {
            _connection = connection;
        }

        public void Publish<T>(T @event, string exchange, string routingKey)
        {
            using var channel = _connection.CreateModel();
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
            channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: null, body: body);
        }
    }
}
