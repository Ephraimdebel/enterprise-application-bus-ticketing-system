public class PaymentCompletedEvent
{
    public Guid PaymentId { get; set; }
    public Guid BookingId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = default!;
    public DateTime CompletedAt { get; set; }
    public string Status { get; set; } = default!;
}
