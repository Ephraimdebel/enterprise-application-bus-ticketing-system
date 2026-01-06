using MediatR;
using Trip.Application.Interfaces;
using Trip.Domain.Aggregates;
using Trip.Domain.Entities;

namespace Trip.Application.Commands.CreateTrip;

public sealed class CreateTripCommandHandler
    : IRequestHandler<CreateTripCommand>
{
    private readonly ITripRepository _tripRepository;

    public CreateTripCommandHandler(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task Handle(
        CreateTripCommand request,
        CancellationToken cancellationToken)
    {

        // Bus and Route will be resolved in Infrastructure
        // or will be passed as references later via events

        var bus = new Bus(request.BusId, "TEMP", "TEMP", 40, "Standard");
        var route = new Route(request.RouteId, "TEMP", "TEMP", 0, TimeSpan.Zero);

        var trip = new Trip(
            request.TripId,
            request.DepartureTime,
            request.ArrivalTime,
            route,
            bus
        );

        await _tripRepository.AddAsync(trip, cancellationToken);
    }
}
