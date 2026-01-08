using BusProvider.Domain.Abstractions;

namespace BusProvider.Domain.ValueObjects;

public sealed class Location : ValueObject
{
    private Location(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Location Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Location is required", nameof(value));
        }

        if (value.Length > 200)
        {
            throw new ArgumentException("Location is too long", nameof(value));
        }

        return new Location(value.Trim());
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value.ToLowerInvariant();
    }

    public override string ToString() => Value;
}
