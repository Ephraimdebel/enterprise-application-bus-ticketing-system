// Payment.Application/Interfaces/IEventPublisher.cs
namespace Payment.Application.Interfaces
{
    public interface IEventPublisher
    {
        void Publish<T>(T @event, string exchange, string routingKey);
    }
}
