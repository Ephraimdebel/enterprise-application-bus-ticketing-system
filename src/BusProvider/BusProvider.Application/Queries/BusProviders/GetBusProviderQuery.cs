using MediatR;

namespace BusProvider.Application.Queries.BusProviders;

public sealed record BusProviderResponse(Guid Id, string Name, string Email, string PhoneNumber, string Address);
public sealed record GetBusProviderQuery(Guid Id) : IRequest<BusProviderResponse?>;
