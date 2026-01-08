using BusProvider.Domain.Abstractions;
using BusProvider.Domain.Events;
using BusProvider.Domain.ValueObjects;

namespace BusProvider.Domain.Aggregates;

public sealed class BusProviderAggregate : AggregateRoot
{
    private BusProviderAggregate()
    {
    }

    private BusProviderAggregate(Guid id, string name, ProviderEmail email, ContactInfo contactInfo)
    {
        Id = id;
        Name = name;
        Email = email;
        ContactInfo = contactInfo;
        RaiseDomainEvent(new BusProviderRegistered(id, name, email.Value, DateTime.UtcNow));
    }

    public string Name { get; private set; } = string.Empty;
    public ProviderEmail Email { get; private set; } = ProviderEmail.Create("placeholder@example.com");
    public ContactInfo ContactInfo { get; private set; } = ContactInfo.Create("0000000000", "placeholder");

    public static BusProviderAggregate Register(string name, string email, string phoneNumber, string address)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Provider name is required", nameof(name));
        }

        return new BusProviderAggregate(Guid.NewGuid(), name.Trim(), ProviderEmail.Create(email), ContactInfo.Create(phoneNumber, address));
    }

    public void Update(string name, string email, string phoneNumber, string address)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Provider name is required", nameof(name));
        }

        Name = name.Trim();
        Email = ProviderEmail.Create(email);
        ContactInfo = ContactInfo.Create(phoneNumber, address);
    }
}
