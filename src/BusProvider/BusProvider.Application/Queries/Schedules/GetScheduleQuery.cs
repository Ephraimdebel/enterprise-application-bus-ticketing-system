using MediatR;

namespace BusProvider.Application.Queries.Schedules;

public sealed record ScheduleResponse(Guid Id, Guid BusId, Guid RouteId, DateOnly TripDate, TimeOnly Departure, TimeOnly Arrival, int SeatsAvailable);
public sealed record GetScheduleQuery(Guid Id) : IRequest<ScheduleResponse?>;
