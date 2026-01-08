using Passenger.Domain.Exceptions;

namespace Passenger.Domain.ValueObjects;

public sealed class Name : ValueObject
{
    public string FirstName { get; }
    public string LastName { get; }

    private Name(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static Name Create(string firstName, string lastName)
    {
        firstName = (firstName ?? string.Empty).Trim();
        lastName = (lastName ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ValidationException("First name is required.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ValidationException("Last name is required.");

        if (firstName.Length > 100)
            throw new ValidationException("First name is too long.");

        if (lastName.Length > 100)
            throw new ValidationException("Last name is too long.");

        return new Name(firstName, lastName);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FirstName.ToUpperInvariant();
        yield return LastName.ToUpperInvariant();
    }

    public override string ToString() => $"{FirstName} {LastName}";
}
