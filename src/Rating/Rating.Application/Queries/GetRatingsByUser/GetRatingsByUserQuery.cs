using MediatR;
using RatingEntity = Rating.Domain.Aggregates.Rating;

namespace Rating.Application.Queries.GetRatingsByUser;

public sealed record GetRatingsByUserQuery(Guid UserId)
    : IRequest<IReadOnlyList<RatingEntity>>;
