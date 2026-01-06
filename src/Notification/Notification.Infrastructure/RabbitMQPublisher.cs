using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Notification.Infrastructure.Messaging
{
    public class RabbitMQPublisher
    {
        private readonly IConnection _connection;

        public RabbitMQPublisher(IConnection connection)
        {
            _connection = connection;
        }

        public void Publish<T>(T message, string exchange, string routingKey)
        {
            using var channel = _connection.CreateModel();
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: null, body: body);
        }
    }
}
