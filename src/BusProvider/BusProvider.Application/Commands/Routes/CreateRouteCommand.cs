using MediatR;

namespace BusProvider.Application.Commands.Routes;

public sealed record CreateRouteCommand(Guid BusId, string Start, string End, double DistanceKm) : IRequest<Guid>;
