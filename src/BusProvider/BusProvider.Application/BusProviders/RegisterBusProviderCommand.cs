using MediatR;

namespace BusProvider.Application.BusProviders;

public sealed record RegisterBusProviderCommand(string Name, string Email, string PhoneNumber, string Address) : IRequest<Guid>;
