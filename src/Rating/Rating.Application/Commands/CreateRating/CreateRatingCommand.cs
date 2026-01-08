using MediatR;

namespace Rating.Application.Commands.CreateRating;

public sealed record CreateRatingCommand(
    Guid TripId,
    Guid UserId,
    Guid TargetId,
    int Stars,
    string? Comment
) : IRequest<Guid>;
