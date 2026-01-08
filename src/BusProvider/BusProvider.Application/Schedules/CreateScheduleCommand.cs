using MediatR;

namespace BusProvider.Application.Schedules;

public sealed record CreateScheduleCommand(Guid BusId, Guid RouteId, DateOnly TripDate, TimeOnly Departure, TimeOnly Arrival, int SeatsAvailable) : IRequest<Guid>;
