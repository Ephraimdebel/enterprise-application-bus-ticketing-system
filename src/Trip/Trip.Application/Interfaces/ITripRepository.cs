using TripAggregate = Trip.Domain.Aggregates.Trip;

namespace Trip.Application.Interfaces;

public interface ITripRepository
{
    Task<TripAggregate?> GetByIdAsync(Guid tripId, CancellationToken cancellationToken);
    Task AddAsync(TripAggregate trip, CancellationToken cancellationToken);
    Task SaveAsync(TripAggregate trip, CancellationToken cancellationToken);
}
