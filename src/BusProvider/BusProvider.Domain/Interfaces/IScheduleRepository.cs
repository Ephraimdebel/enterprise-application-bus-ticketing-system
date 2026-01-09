using BusProvider.Domain.Aggregates;

namespace BusProvider.Domain.Interfaces;

public interface IScheduleRepository
{
    Task AddAsync(ScheduleAggregate schedule, CancellationToken cancellationToken = default);
    Task<ScheduleAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<ScheduleAggregate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<ScheduleAggregate>> GetByRouteAsync(Guid routeId, CancellationToken cancellationToken = default);
    Task<List<ScheduleAggregate>> GetByBusAsync(Guid busId, CancellationToken cancellationToken = default);
    Task RemoveAsync(ScheduleAggregate schedule, CancellationToken cancellationToken = default);
}
