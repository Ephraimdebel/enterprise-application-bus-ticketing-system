using MediatR;

namespace BusProvider.Application.Routes;

public sealed record ListRoutesQuery(Guid? BusId) : IRequest<List<RouteResponse>>;

public sealed record UpdateRouteCommand(Guid Id, string Start, string End, double DistanceKm) : IRequest<bool>;

public sealed record DeleteRouteCommand(Guid Id) : IRequest<bool>;
