using BusProvider.Domain.Abstractions;

namespace BusProvider.Domain.ValueObjects;

public sealed class TravelDate : ValueObject
{
    private TravelDate(DateOnly value)
    {
        Value = value;
    }

    public DateOnly Value { get; }

    public static TravelDate Create(DateOnly value)
    {
        if (value == default)
        {
            throw new ArgumentException("Travel date is required", nameof(value));
        }

        return new TravelDate(value);
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString("O");
}
