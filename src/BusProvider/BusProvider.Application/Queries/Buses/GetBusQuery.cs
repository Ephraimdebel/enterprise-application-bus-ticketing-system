using MediatR;

namespace BusProvider.Application.Queries.Buses;

public sealed record GetBusQuery(Guid BusId) : IRequest<BusResponse?>;

public sealed record BusResponse(Guid Id, Guid ProviderId, string BusNumber, string BusType, int SeatCapacity);
