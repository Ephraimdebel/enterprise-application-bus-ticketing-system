using BusProvider.Domain.Abstractions;

namespace BusProvider.Domain.ValueObjects;

public sealed class BusType : ValueObject
{
    private BusType(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static BusType Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Bus type is required", nameof(value));
        }

        return new BusType(value.Trim());
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value.ToLowerInvariant();
    }

    public override string ToString() => Value;
}
