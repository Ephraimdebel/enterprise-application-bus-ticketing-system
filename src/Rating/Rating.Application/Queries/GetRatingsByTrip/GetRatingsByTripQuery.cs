using MediatR;
using RatingEntity = Rating.Domain.Aggregates.Rating;

namespace Rating.Application.Queries.GetRatingsByTrip;

public sealed record GetRatingsByTripQuery(Guid TripId)
    : IRequest<IReadOnlyList<RatingEntity>>;
