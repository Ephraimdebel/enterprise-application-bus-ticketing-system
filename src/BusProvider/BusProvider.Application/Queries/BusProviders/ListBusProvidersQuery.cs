using MediatR;

namespace BusProvider.Application.Queries.BusProviders;

public sealed record ListBusProvidersQuery() : IRequest<List<BusProviderResponse>>;
