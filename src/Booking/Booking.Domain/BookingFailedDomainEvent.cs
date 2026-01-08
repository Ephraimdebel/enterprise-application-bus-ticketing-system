namespace Booking.Domain;

public sealed record BookingFailedDomainEvent(Guid BookingId, Guid PassengerId) : IDomainEvent;
