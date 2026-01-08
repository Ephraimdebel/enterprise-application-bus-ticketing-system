using Microsoft.EntityFrameworkCore;
using Rating.Application.Interfaces;
using RatingEntity = Rating.Domain.Aggregates.Rating;
using Rating.Infrastructure.Persistence;

namespace Rating.Infrastructure.Repositories;

public sealed class RatingRepository : IRatingRepository
{
    private readonly RatingDbContext _context;

    public RatingRepository(RatingDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RatingEntity rating, CancellationToken cancellationToken = default)
    {
        await _context.Ratings.AddAsync(rating, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RatingEntity>> GetByTripIdAsync(Guid tripId, CancellationToken cancellationToken = default)
    {
        return await _context.Ratings
            .Where(r => r.TripId == tripId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RatingEntity>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Ratings
            .Where(r => r.UserId == userId)
            .ToListAsync(cancellationToken);
    }
}

