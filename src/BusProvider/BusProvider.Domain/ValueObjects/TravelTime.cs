using BusProvider.Domain.Abstractions;

namespace BusProvider.Domain.ValueObjects;

public sealed class TravelTime : ValueObject
{
    private TravelTime(TimeOnly value)
    {
        Value = value;
    }

    public TimeOnly Value { get; }

    public static TravelTime Create(TimeOnly value)
    {
        return new TravelTime(value);
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString("HH:mm:ss");
}
