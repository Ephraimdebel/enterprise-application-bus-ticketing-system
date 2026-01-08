namespace Booking.Domain;

public record Money(decimal Amount, string Currency)
{
    public static Money Zero() => new(0, "USD");
    public static Money Zero(string currency) => new(0, currency);

    public static Money operator +(Money first, Money second)
    {
        if (first.Currency != second.Currency)
        {
            throw new InvalidOperationException("Currencies must be the same");
        }

        return new Money(first.Amount + second.Amount, first.Currency);
    }
}
