
namespace Booking.Domain;

public record BookingConfirmedDomainEvent(Guid BookingId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
