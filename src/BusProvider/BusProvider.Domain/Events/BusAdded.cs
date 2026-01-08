using BusProvider.Domain.Abstractions;

namespace BusProvider.Domain.Events;

public sealed record BusAdded(Guid BusId, Guid ProviderId, string BusNumber, string BusType, int SeatCapacity, DateTime OccurredOnUtc)
    : DomainEvent(Guid.NewGuid(), OccurredOnUtc);
