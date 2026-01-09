using Payment.Domain.ValueObjects;

namespace Payment.Domain.Entities;

public class PaymentEntity
{
    public Guid Id { get; private set; }
    public Guid BookingId { get; private set; }
    public Money Amount { get; private set; }
    public PaymentMethod Method { get; private set; }
    public string Status { get; private set; } // Pending, Success, Failed
    public DateTime CreatedAt { get; private set; }

    private readonly List<global::Payment.Domain.Events.IDomainEvent> _domainEvents = new();
    public IReadOnlyList<global::Payment.Domain.Events.IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
    public void ClearDomainEvents() => _domainEvents.Clear();

    private PaymentEntity() { } // EF needs it later

    public PaymentEntity(Guid bookingId, Money amount, PaymentMethod method)
    {
        Id = Guid.NewGuid();
        BookingId = bookingId;
        Amount = amount;
        Method = method;
        Status = "Pending";
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsSuccess()
    {
        Status = "Confirmed";
        _domainEvents.Add(new global::Payment.Domain.Events.PaymentCompleted(Id, BookingId, Amount.Amount, Amount.Currency));
    }

    public void MarkAsFailed()
    {
        Status = "Failed";
    }
}
