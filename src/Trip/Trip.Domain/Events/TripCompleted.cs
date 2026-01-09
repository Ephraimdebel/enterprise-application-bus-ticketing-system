namespace Trip.Domain.Events;

public sealed record TripCompleted(Guid TripId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
