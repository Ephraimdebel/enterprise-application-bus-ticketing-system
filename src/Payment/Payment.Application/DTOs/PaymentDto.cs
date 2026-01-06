namespace Payment.Application.DTOs;

public class PaymentDto
{
    public Guid? Id { get; set; }
    public Guid BookingId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string? Status { get; set; }
}
