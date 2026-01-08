
namespace Booking.Domain;

public record BookingConfirmedDomainEvent(Guid BookingId,  decimal TotalAmount) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
