namespace BusProvider.Domain.Abstractions;

public abstract record DomainEvent(Guid Id, DateTime OccurredOnUtc);
