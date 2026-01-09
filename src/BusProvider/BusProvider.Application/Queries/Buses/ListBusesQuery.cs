using MediatR;

namespace BusProvider.Application.Queries.Buses;

public sealed record ListBusesQuery(Guid? ProviderId) : IRequest<List<BusResponse>>;
