using Microsoft.EntityFrameworkCore;
using Trip.Application.Interfaces;
using TripAggregate = Trip.Domain.Aggregates.Trip;
using Trip.Infrastructure.Persistence;

namespace Trip.Infrastructure.Repositories;

public sealed class TripRepository : ITripRepository
{
    private readonly TripDbContext _context;

    public TripRepository(TripDbContext context)
    {
        _context = context;
    }

    public async Task<TripAggregate?> GetByIdAsync(Guid tripId, CancellationToken ct)
    {
        return await _context.Trips
            .Include(t => t.Seats)
            .FirstOrDefaultAsync(t => t.TripId == tripId, ct);
    }

    public async Task AddAsync(TripAggregate trip, CancellationToken ct)
    {
        await _context.Trips.AddAsync(trip, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task SaveAsync(TripAggregate trip, CancellationToken ct)
    {
        _context.Trips.Update(trip);
        await _context.SaveChangesAsync(ct);
    }
}
