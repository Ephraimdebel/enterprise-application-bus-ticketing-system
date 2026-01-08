namespace Trip.Domain.Events;

public sealed class TripCancelled : IDomainEvent
{
    public Guid TripId { get; }
    public DateTime OccurredOn { get; }

    public TripCancelled(Guid tripId)
    {
        TripId = tripId;
        OccurredOn = DateTime.UtcNow;
    }
}
