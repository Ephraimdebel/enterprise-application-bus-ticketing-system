using BusProvider.Domain.Interfaces;
using MediatR;

namespace BusProvider.Application.Queries.Buses;

public sealed class ListBusesQueryHandler : IRequestHandler<ListBusesQuery, List<BusResponse>>
{
    private readonly IBusRepository _repository;

    public ListBusesQueryHandler(IBusRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<BusResponse>> Handle(ListBusesQuery request, CancellationToken cancellationToken)
    {
        var buses = request.ProviderId.HasValue
            ? await _repository.GetByProviderAsync(request.ProviderId.Value, cancellationToken)
            : await _repository.GetAllAsync(cancellationToken);

        return buses
            .Select(b => new BusResponse(b.Id, b.ProviderId, b.BusNumber.Value, b.BusType.Value, b.SeatCapacity.Value))
            .ToList();
    }
}
