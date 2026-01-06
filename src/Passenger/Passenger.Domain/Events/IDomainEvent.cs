namespace Passenger.Domain.Events;

public interface IDomainEvent
{
    DateTime OccurredOnUtc { get; }
}

/// <summary>
/// Marker interface for aggregates that expose domain events.
/// Infrastructure can collect these and store/publish via outbox.
/// </summary>
public interface IHasDomainEvents
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    IReadOnlyCollection<IDomainEvent> DequeueDomainEvents();
}
