using MediatR;

namespace BusProvider.Application.Commands.BusProviders;

public sealed record UpdateBusProviderCommand(Guid Id, string Name, string Email, string PhoneNumber, string Address) : IRequest<bool>;
