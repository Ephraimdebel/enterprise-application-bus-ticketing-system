namespace Booking.Domain;

public sealed record BookingFailedDomainEvent(Guid BookingId) : IDomainEvent;
