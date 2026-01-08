using BusProvider.Domain.Aggregates;

namespace BusProvider.Domain.Interfaces;

public interface IRouteRepository
{
    Task AddAsync(RouteAggregate route, CancellationToken cancellationToken = default);
    Task<RouteAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<RouteAggregate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<RouteAggregate>> GetByBusAsync(Guid busId, CancellationToken cancellationToken = default);
    Task RemoveAsync(RouteAggregate route, CancellationToken cancellationToken = default);
}
