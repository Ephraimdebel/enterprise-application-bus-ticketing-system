using MediatR;

namespace BusProvider.Application.Commands.BusProviders;

public sealed record RegisterBusProviderCommand(string Name, string Email, string PhoneNumber, string Address) : IRequest<Guid>;
