using MediatR;

namespace BusProvider.Application.Routes;

public sealed record CreateRouteCommand(Guid BusId, string Start, string End, double DistanceKm) : IRequest<Guid>;
