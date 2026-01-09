using MediatR;

namespace BusProvider.Application.Queries.Schedules;

public sealed record ListSchedulesQuery(Guid? BusId, Guid? RouteId) : IRequest<List<ScheduleResponse>>;
