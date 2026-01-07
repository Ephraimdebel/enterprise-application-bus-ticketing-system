using MediatR;

namespace BusProvider.Application.Buses;

public sealed record ListBusesQuery(Guid? ProviderId) : IRequest<List<BusResponse>>;

public sealed record UpdateBusCommand(Guid Id, string BusNumber, string BusType, int SeatCapacity) : IRequest<bool>;

public sealed record DeleteBusCommand(Guid Id) : IRequest<bool>;
