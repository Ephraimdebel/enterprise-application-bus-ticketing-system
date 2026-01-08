using System.Net.Mail;
using Passenger.Domain.Exceptions;

namespace Passenger.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email Create(string value)
    {
        value = (value ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException("Email is required.");

        // MailAddress provides reasonable validation for most cases
        try
        {
            var addr = new MailAddress(value);
            if (!string.Equals(addr.Address, value, StringComparison.OrdinalIgnoreCase))
                throw new ValidationException("Email is invalid.");
        }
        catch
        {
            throw new ValidationException("Email is invalid.");
        }

        // normalize
        value = value.ToLowerInvariant();
        return new Email(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value.ToLowerInvariant();
    }

    public override string ToString() => Value;
}
