namespace Payment.Domain.Events;

public record PaymentCompleted(Guid PaymentId, Guid BookingId, decimal Amount);
