using Trip.Domain.Aggregates;

namespace Trip.Application.Interfaces;

public interface ITripRepository
{
    Task<Trip?> GetByIdAsync(Guid tripId, CancellationToken cancellationToken);
    Task AddAsync(Trip trip, CancellationToken cancellationToken);
    Task SaveAsync(Trip trip, CancellationToken cancellationToken);
}
