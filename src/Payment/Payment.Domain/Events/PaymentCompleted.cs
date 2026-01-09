namespace Payment.Domain.Events;

public sealed record PaymentCompleted(Guid PaymentId, Guid BookingId, decimal Amount, string Currency) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
