using Trip.Application.DTOs;

namespace Trip.Application.Interfaces;

public interface IBusProviderGateway
{
    Task<BusDto?> GetBusAsync(Guid busId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RouteDto>> ListRoutesAsync(Guid? busId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ScheduleDto>> ListSchedulesAsync(Guid? busId, Guid? routeId, CancellationToken cancellationToken = default);
}
