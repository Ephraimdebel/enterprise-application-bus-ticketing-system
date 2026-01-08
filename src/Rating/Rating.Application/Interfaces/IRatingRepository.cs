using RatingEntity = Rating.Domain.Aggregates.Rating;


namespace Rating.Application.Interfaces;

public interface IRatingRepository
{
    Task AddAsync(RatingEntity rating, CancellationToken cancellationToken);

    Task<IReadOnlyList<RatingEntity>> GetByTripIdAsync(
        Guid tripId,
        CancellationToken cancellationToken
    );

    Task<IReadOnlyList<RatingEntity>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken
    );
}

