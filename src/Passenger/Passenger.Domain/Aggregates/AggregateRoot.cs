using Passenger.Domain.Entities;
using Passenger.Domain.Events;

namespace Passenger.Domain.Aggregates;

public abstract class AggregateRoot<TId> : Entity<TId>, IHasDomainEvents
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();

    protected AggregateRoot(TId id) : base(id) { }

    protected AggregateRoot() { }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void RaiseDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);

    public IReadOnlyCollection<IDomainEvent> DequeueDomainEvents()
    {
        if (_domainEvents.Count == 0) return Array.Empty<IDomainEvent>();

        var events = _domainEvents.ToArray();
        _domainEvents.Clear();
        return events;
    }
}
