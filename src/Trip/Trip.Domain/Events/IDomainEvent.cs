namespace Trip.Domain.Events;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
