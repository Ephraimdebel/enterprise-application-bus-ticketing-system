using BusProvider.Domain.Repositories;
using MediatR;

namespace BusProvider.Application.Buses;

public sealed class GetBusQueryHandler : IRequestHandler<GetBusQuery, BusResponse?>
{
    private readonly IBusRepository _busRepository;

    public GetBusQueryHandler(IBusRepository busRepository)
    {
        _busRepository = busRepository;
    }

    public async Task<BusResponse?> Handle(GetBusQuery request, CancellationToken cancellationToken)
    {
        var bus = await _busRepository.GetByIdAsync(request.BusId, cancellationToken);
        if (bus is null) return null;

        return new BusResponse(bus.Id, bus.ProviderId, bus.BusNumber.Value, bus.BusType.Value, bus.SeatCapacity.Value);
    }
}
