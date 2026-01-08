using RatingEntity = Rating.Domain.Aggregates.Rating;

namespace Rating.Application.Interfaces;

public interface IRatingRepository
{
    Task AddAsync(RatingEntity rating, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RatingEntity>> GetByTripIdAsync(Guid tripId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RatingEntity>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
