using BusProvider.Domain.Abstractions;

namespace BusProvider.Domain.Events;

public sealed record RouteCreated(Guid RouteId, Guid BusId, string Start, string End, double DistanceKm, DateTime OccurredOnUtc)
    : DomainEvent(Guid.NewGuid(), OccurredOnUtc);
