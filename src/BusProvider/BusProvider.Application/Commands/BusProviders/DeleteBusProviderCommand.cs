using MediatR;

namespace BusProvider.Application.Commands.BusProviders;

public sealed record DeleteBusProviderCommand(Guid Id) : IRequest<bool>;
