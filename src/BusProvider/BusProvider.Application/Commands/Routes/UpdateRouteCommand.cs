using MediatR;

namespace BusProvider.Application.Commands.Routes;

public sealed record UpdateRouteCommand(Guid Id, string Start, string End, double DistanceKm) : IRequest<bool>;
