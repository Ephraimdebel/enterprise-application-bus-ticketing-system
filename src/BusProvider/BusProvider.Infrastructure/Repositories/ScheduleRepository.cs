using BusProvider.Domain.Aggregates;
using BusProvider.Domain.Interfaces;
using BusProvider.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BusProvider.Infrastructure.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly BusProviderDbContext _dbContext;

    public ScheduleRepository(BusProviderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AddAsync(ScheduleAggregate schedule, CancellationToken cancellationToken = default)
    {
        _dbContext.Schedules.Add(schedule);
        return Task.CompletedTask;
    }

    public Task<ScheduleAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Schedules.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public Task<List<ScheduleAggregate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.Schedules.AsNoTracking().ToListAsync(cancellationToken);
    }

    public Task<List<ScheduleAggregate>> GetByRouteAsync(Guid routeId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Schedules.AsNoTracking().Where(s => s.RouteId == routeId).ToListAsync(cancellationToken);
    }

    public Task<List<ScheduleAggregate>> GetByBusAsync(Guid busId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Schedules.AsNoTracking().Where(s => s.BusId == busId).ToListAsync(cancellationToken);
    }

    public Task RemoveAsync(ScheduleAggregate schedule, CancellationToken cancellationToken = default)
    {
        _dbContext.Schedules.Remove(schedule);
        return Task.CompletedTask;
    }
}
