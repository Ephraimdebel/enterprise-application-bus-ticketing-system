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
    }

    public void MarkAsFailed()
    {
        Status = "Failed";
    }
}
