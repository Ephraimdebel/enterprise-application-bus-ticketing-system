namespace Booking.Domain;

public abstract class Entity
{
    protected Entity() { } // Added for EF Core

    protected Entity(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; init; }

    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
