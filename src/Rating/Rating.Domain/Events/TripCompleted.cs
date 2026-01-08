namespace Rating.Domain.Events;

public sealed class TripCompleted
{
    public Guid TripId { get; }
    public Guid UserId { get; }

    public TripCompleted(Guid tripId, Guid userId)
    {
        if (tripId == Guid.Empty)
            throw new InvalidOperationException("TripId cannot be empty.");

        if (userId == Guid.Empty)
            throw new InvalidOperationException("UserId cannot be empty.");

        TripId = tripId;
        UserId = userId;
    }
}

