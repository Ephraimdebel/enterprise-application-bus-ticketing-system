namespace Payment.Domain.Events;

public record PaymentFailed(Guid PaymentId, Guid BookingId, string Reason);
