using MediatR;

namespace BusProvider.Application.BusProviders;

public sealed record BusProviderResponse(Guid Id, string Name, string Email, string PhoneNumber, string Address);

public sealed record GetBusProviderQuery(Guid Id) : IRequest<BusProviderResponse?>;

public sealed record ListBusProvidersQuery() : IRequest<List<BusProviderResponse>>;

public sealed record UpdateBusProviderCommand(Guid Id, string Name, string Email, string PhoneNumber, string Address) : IRequest<bool>;

public sealed record DeleteBusProviderCommand(Guid Id) : IRequest<bool>;
