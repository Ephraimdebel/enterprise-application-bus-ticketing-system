using BusProvider.Domain.Abstractions;

namespace BusProvider.Domain.ValueObjects;

public sealed class BusNumber : ValueObject
{
    private BusNumber(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static BusNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Bus number is required", nameof(value));
        }

        if (value.Length > 32)
        {
            throw new ArgumentException("Bus number is too long", nameof(value));
        }

        return new BusNumber(value.Trim());
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value.ToLowerInvariant();
    }

    public override string ToString() => Value;
}
