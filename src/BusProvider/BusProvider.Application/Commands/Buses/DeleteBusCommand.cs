using MediatR;

namespace BusProvider.Application.Commands.Buses;

public sealed record DeleteBusCommand(Guid Id) : IRequest<bool>;
