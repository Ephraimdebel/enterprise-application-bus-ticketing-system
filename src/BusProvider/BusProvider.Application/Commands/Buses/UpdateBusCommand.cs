using MediatR;

namespace BusProvider.Application.Commands.Buses;

public sealed record UpdateBusCommand(Guid Id, string BusNumber, string BusType, int SeatCapacity) : IRequest<bool>;
