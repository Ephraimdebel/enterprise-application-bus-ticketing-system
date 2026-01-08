using MediatR;

namespace BusProvider.Application.Queries.Routes;

public sealed record ListRoutesQuery(Guid? BusId) : IRequest<List<RouteResponse>>;
