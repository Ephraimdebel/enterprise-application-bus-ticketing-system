using BusProvider.Domain.Abstractions;

namespace BusProvider.Domain.Events;

public sealed record TripPublished(Guid ScheduleId, Guid BusId, Guid RouteId, DateOnly TripDate, TimeOnly Departure, TimeOnly Arrival, int SeatsAvailable, DateTime OccurredOnUtc)
    : DomainEvent(Guid.NewGuid(), OccurredOnUtc);
