using MediatR;

namespace BusProvider.Application.Routes;

public sealed record GetRouteQuery(Guid RouteId) : IRequest<RouteResponse?>;

public sealed record RouteResponse(Guid Id, Guid BusId, string Start, string End, double DistanceKm);
