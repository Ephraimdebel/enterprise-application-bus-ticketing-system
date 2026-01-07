using MediatR;

namespace BusProvider.Application.Buses;

public sealed record CreateBusCommand(Guid ProviderId, string BusNumber, string BusType, int SeatCapacity) : IRequest<Guid>;
