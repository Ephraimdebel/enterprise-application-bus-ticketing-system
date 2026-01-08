using System.Text.RegularExpressions;
using BusProvider.Domain.Abstractions;

namespace BusProvider.Domain.ValueObjects;

public sealed class ProviderEmail : ValueObject
{
    private static readonly Regex EmailRegex =
        new("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private ProviderEmail(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static ProviderEmail Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email is required", nameof(value));
        }

        if (!EmailRegex.IsMatch(value))
        {
            throw new ArgumentException("Invalid email format", nameof(value));
        }

        return new ProviderEmail(value.Trim());
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value.ToLowerInvariant();
    }

    public override string ToString() => Value;
}
