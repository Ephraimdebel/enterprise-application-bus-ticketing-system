using MediatR;
using Trip.Application.Interfaces;

namespace Trip.Application.Commands.UpdateTrip;

public sealed class UpdateTripCommandHandler
    : IRequestHandler<UpdateTripCommand, Unit>
{
    private readonly ITripRepository _tripRepository;

    public UpdateTripCommandHandler(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task<Unit> Handle(
        UpdateTripCommand request,
        CancellationToken cancellationToken)
    {
        var trip = await _tripRepository.GetByIdAsync(request.TripId, cancellationToken)
            ?? throw new InvalidOperationException("Trip not found.");

        trip.UpdateSchedule(
            request.NewDepartureTime,
            request.NewArrivalTime,
            request.NewPrice
        );

        await _tripRepository.SaveAsync(trip, cancellationToken);

        return Unit.Value;
    }
}
