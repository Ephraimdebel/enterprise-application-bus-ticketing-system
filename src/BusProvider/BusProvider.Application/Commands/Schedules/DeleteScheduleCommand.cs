using MediatR;

namespace BusProvider.Application.Commands.Schedules;

public sealed record DeleteScheduleCommand(Guid Id) : IRequest<bool>;
