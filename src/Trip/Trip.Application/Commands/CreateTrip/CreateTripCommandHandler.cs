using MediatR;
using Trip.Application.Interfaces;
using TripAggregates = Trip.Domain.Aggregates.Trip;

namespace Trip.Application.Commands.CreateTrip;

public sealed class CreateTripCommandHandler
    : IRequestHandler<CreateTripCommand, Unit>
{
    private readonly ITripRepository _tripRepository;

    public CreateTripCommandHandler(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task<Unit> Handle(
        CreateTripCommand request,
        CancellationToken cancellationToken)
    {
        var trip = new TripAggregates(
            request.TripId,
            request.DepartureTime,
            request.ArrivalTime,
            request.BusId,
            request.RouteId,
            request.SeatCapacity, // placeholder
            request.Price 
        );

        await _tripRepository.AddAsync(trip, cancellationToken);

        return Unit.Value;
    }
}
