using MediatR;

namespace BusProvider.Application.Commands.Schedules;

public sealed record UpdateScheduleCommand(Guid Id, DateOnly TripDate, TimeOnly Departure, TimeOnly Arrival, int SeatsAvailable) : IRequest<bool>;
