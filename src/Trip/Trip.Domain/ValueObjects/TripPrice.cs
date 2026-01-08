namespace Trip.Domain.ValueObjects;

public sealed class TripPrice
{
    public decimal Amount { get; private set; }
    private TripPrice() { }
    public TripPrice(decimal amount)
    {
        if (amount < 0) throw new ArgumentException("Price cannot be negative");
        Amount = amount;
    }
}
