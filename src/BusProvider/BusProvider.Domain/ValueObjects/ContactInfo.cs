using BusProvider.Domain.Abstractions;

namespace BusProvider.Domain.ValueObjects;

public sealed class ContactInfo : ValueObject
{
    private ContactInfo(string phoneNumber, string address)
    {
        PhoneNumber = phoneNumber;
        Address = address;
    }

    public string PhoneNumber { get; }
    public string Address { get; }

    public static ContactInfo Create(string phoneNumber, string address)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            throw new ArgumentException("Phone number is required", nameof(phoneNumber));
        }

        if (string.IsNullOrWhiteSpace(address))
        {
            throw new ArgumentException("Address is required", nameof(address));
        }

        return new ContactInfo(phoneNumber.Trim(), address.Trim());
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return PhoneNumber.ToLowerInvariant();
        yield return Address.ToLowerInvariant();
    }
}
