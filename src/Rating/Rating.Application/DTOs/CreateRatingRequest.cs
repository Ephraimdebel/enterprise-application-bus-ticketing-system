namespace Rating.Application.DTOs;

public sealed class CreateRatingRequest
{
    public Guid TripId { get; init; }
    public Guid UserId { get; init; }
    public Guid TargetId { get; init; }
    public int Stars { get; init; }
    public string? Comment { get; init; }
}
