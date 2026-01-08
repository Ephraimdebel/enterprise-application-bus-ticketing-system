using BusProvider.Domain.Interfaces;
using MediatR;

namespace BusProvider.Application.Queries.Routes;

public sealed class ListRoutesQueryHandler : IRequestHandler<ListRoutesQuery, List<RouteResponse>>
{
    private readonly IRouteRepository _repository;

    public ListRoutesQueryHandler(IRouteRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<RouteResponse>> Handle(ListRoutesQuery request, CancellationToken cancellationToken)
    {
        var routes = request.BusId.HasValue
            ? await _repository.GetByBusAsync(request.BusId.Value, cancellationToken)
            : await _repository.GetAllAsync(cancellationToken);

        return routes
            .Select(r => new RouteResponse(r.Id, r.BusId, r.Start.Value, r.End.Value, r.Distance.Kilometers))
            .ToList();
    }
}
