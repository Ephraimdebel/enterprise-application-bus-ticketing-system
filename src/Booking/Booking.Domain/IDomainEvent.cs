using MediatR;
namespace Booking.Domain;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
