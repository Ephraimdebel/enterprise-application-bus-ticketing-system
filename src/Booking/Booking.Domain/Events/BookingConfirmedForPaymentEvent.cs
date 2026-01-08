using global::Booking.Domain;

// This is a new record event used to notify Payment module
public record BookingConfirmedForPaymentEvent(Guid BookingId, decimal TotalAmount)
    : BookingConfirmedDomainEvent(BookingId, TotalAmount);
