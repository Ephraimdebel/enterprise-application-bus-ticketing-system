using BusProvider.Domain.Repositories;
using MediatR;

namespace BusProvider.Application.Routes;

public sealed class GetRouteQueryHandler : IRequestHandler<GetRouteQuery, RouteResponse?>
{
    private readonly IRouteRepository _routeRepository;

    public GetRouteQueryHandler(IRouteRepository routeRepository)
    {
        _routeRepository = routeRepository;
    }

    public async Task<RouteResponse?> Handle(GetRouteQuery request, CancellationToken cancellationToken)
    {
        var route = await _routeRepository.GetByIdAsync(request.RouteId, cancellationToken);
        if (route is null) return null;

        return new RouteResponse(route.Id, route.BusId, route.Start.Value, route.End.Value, route.Distance.Kilometers);
    }
}
