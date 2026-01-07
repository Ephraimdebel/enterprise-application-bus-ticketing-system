using MediatR;

namespace BusProvider.Application.Schedules;

public sealed record ScheduleResponse(Guid Id, Guid BusId, Guid RouteId, DateOnly TripDate, TimeOnly Departure, TimeOnly Arrival, int SeatsAvailable);

public sealed record GetScheduleQuery(Guid Id) : IRequest<ScheduleResponse?>;

public sealed record ListSchedulesQuery(Guid? BusId, Guid? RouteId) : IRequest<List<ScheduleResponse>>;

public sealed record UpdateScheduleCommand(Guid Id, DateOnly TripDate, TimeOnly Departure, TimeOnly Arrival, int SeatsAvailable) : IRequest<bool>;

public sealed record DeleteScheduleCommand(Guid Id) : IRequest<bool>;
