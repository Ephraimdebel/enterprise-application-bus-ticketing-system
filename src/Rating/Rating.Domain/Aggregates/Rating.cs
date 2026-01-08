using Rating.Domain.ValueObjects;

namespace Rating.Domain.Aggregates;

public sealed class Rating
{
    public Guid Id { get; private set; }
    public Guid TripId { get; private set; }
    public Guid UserId { get; private set; }
    public Guid TargetId { get; private set; }
    public Score Stars { get; private set; }
    public string? Comment { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Rating() { }

    public Rating(
        Guid id,
        Guid tripId,
        Guid userId,
        Guid targetId,
        Score stars,
        string? comment
    )
    {
        if (tripId == Guid.Empty)
            throw new InvalidOperationException("TripId cannot be empty.");

        if (userId == Guid.Empty)
            throw new InvalidOperationException("UserId cannot be empty.");

        if (targetId == Guid.Empty)
            throw new InvalidOperationException("TargetId cannot be empty.");

        Id = id;
        TripId = tripId;
        UserId = userId;
        TargetId = targetId;
        Stars = stars;
        Comment = comment;
        CreatedAt = DateTime.UtcNow;
    }
}

