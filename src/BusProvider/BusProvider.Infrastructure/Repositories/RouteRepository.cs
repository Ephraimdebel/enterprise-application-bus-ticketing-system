using BusProvider.Domain.Aggregates;
using BusProvider.Domain.Interfaces;
using BusProvider.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BusProvider.Infrastructure.Repositories;

public class RouteRepository : IRouteRepository
{
    private readonly BusProviderDbContext _dbContext;

    public RouteRepository(BusProviderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AddAsync(RouteAggregate route, CancellationToken cancellationToken = default)
    {
        _dbContext.Routes.Add(route);
        return Task.CompletedTask;
    }

    public Task<RouteAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Routes.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public Task<List<RouteAggregate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.Routes.AsNoTracking().ToListAsync(cancellationToken);
    }

    public Task<List<RouteAggregate>> GetByBusAsync(Guid busId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Routes.AsNoTracking().Where(r => r.BusId == busId).ToListAsync(cancellationToken);
    }

    public Task RemoveAsync(RouteAggregate route, CancellationToken cancellationToken = default)
    {
        _dbContext.Routes.Remove(route);
        return Task.CompletedTask;
    }
}
