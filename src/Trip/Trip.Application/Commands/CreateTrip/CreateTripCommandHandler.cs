using MediatR;
using Trip.Application.Interfaces;
using TripAggregates = Trip.Domain.Aggregates.Trip;

namespace Trip.Application.Commands.CreateTrip;

public sealed class CreateTripCommandHandler
    : IRequestHandler<CreateTripCommand, Unit>
{
    private readonly ITripRepository _tripRepository;
    private readonly IBusProviderGateway _busProviderGateway;

    public CreateTripCommandHandler(ITripRepository tripRepository, IBusProviderGateway busProviderGateway)
    {
        _tripRepository = tripRepository;
        _busProviderGateway = busProviderGateway;
    }

    public async Task<Unit> Handle(
        CreateTripCommand request,
        CancellationToken cancellationToken)
    {
        var bus = await _busProviderGateway.GetBusAsync(request.BusId, cancellationToken);
        if (bus is null)
        {
            throw new InvalidOperationException("Bus not found for trip creation.");
        }

        var trip = new TripAggregates(
            request.TripId,
            request.DepartureTime,
            request.ArrivalTime,
            request.BusId,
            request.RouteId,
            bus.SeatCapacity,
            request.Price 
        );

        await _tripRepository.AddAsync(trip, cancellationToken);

        return Unit.Value;
    }
}
