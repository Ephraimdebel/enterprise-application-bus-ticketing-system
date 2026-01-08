using MediatR;
using Rating.Application.Interfaces;
using RatingEntity = Rating.Domain.Aggregates.Rating;

namespace Rating.Application.Queries.GetRatingsByUser;

public sealed class GetRatingsByUserQueryHandler
    : IRequestHandler<GetRatingsByUserQuery, IReadOnlyList<RatingEntity>>
{
    private readonly IRatingRepository _repository;

    public GetRatingsByUserQueryHandler(IRatingRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<RatingEntity>> Handle(
        GetRatingsByUserQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _repository.GetByUserIdAsync(
            request.UserId,
            cancellationToken
        );
    }
}
