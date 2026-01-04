using BusProvider.Domain.Abstractions;

namespace BusProvider.Domain.ValueObjects;

public sealed class SeatCapacity : ValueObject
{
    private SeatCapacity(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static SeatCapacity Create(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentException("Seat capacity must be greater than zero", nameof(value));
        }

        return new SeatCapacity(value);
    }

    public SeatCapacity Decrease(int count)
    {
        if (count < 0)
        {
            throw new ArgumentException("Decrease must be non-negative", nameof(count));
        }

        if (count > Value)
        {
            throw new InvalidOperationException("Cannot decrease below zero");
        }

        return new SeatCapacity(Value - count);
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
