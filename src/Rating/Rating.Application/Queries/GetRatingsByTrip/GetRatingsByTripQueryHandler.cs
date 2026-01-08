using MediatR;
using Rating.Application.Interfaces;
using RatingEntity = Rating.Domain.Aggregates.Rating;

namespace Rating.Application.Queries.GetRatingsByTrip;

public sealed class GetRatingsByTripQueryHandler
    : IRequestHandler<GetRatingsByTripQuery, IReadOnlyList<RatingEntity>>
{
    private readonly IRatingRepository _repository;

    public GetRatingsByTripQueryHandler(IRatingRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<RatingEntity>> Handle(
        GetRatingsByTripQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _repository.GetByTripIdAsync(
            request.TripId,
            cancellationToken
        );
    }
}
