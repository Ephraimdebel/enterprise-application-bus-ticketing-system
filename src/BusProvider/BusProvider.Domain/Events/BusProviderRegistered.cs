using BusProvider.Domain.Abstractions;

namespace BusProvider.Domain.Events;

public sealed record BusProviderRegistered(Guid ProviderId, string Name, string Email, DateTime OccurredOnUtc)
    : DomainEvent(Guid.NewGuid(), OccurredOnUtc);
