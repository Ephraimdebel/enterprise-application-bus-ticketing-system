
namespace Booking.Domain;

public record BookingReservedDomainEvent(Guid BookingId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
