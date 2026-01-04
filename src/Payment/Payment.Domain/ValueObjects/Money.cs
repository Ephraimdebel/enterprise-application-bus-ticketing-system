namespace Payment.Domain.ValueObjects;

public class Money
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    private Money() {}   // For EF

    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }
}
