using MediatR;

namespace BusProvider.Application.Commands.Routes;

public sealed record DeleteRouteCommand(Guid Id) : IRequest<bool>;
