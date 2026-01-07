
namespace Booking.Domain;

public record BookingCancelledDomainEvent(Guid BookingId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
