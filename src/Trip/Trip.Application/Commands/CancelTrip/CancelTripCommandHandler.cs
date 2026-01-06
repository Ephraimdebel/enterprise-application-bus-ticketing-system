using MediatR;
using Trip.Application.Interfaces;

namespace Trip.Application.Commands.CancelTrip;

public sealed class CancelTripCommandHandler
    : IRequestHandler<CancelTripCommand>
{
    private readonly ITripRepository _tripRepository;

    public CancelTripCommandHandler(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task Handle(
        CancelTripCommand request,
        CancellationToken cancellationToken)
    {
        var trip = await _tripRepository
            .GetByIdAsync(request.TripId, cancellationToken);

        if (trip is null)
            throw new InvalidOperationException("Trip not found.");

        trip.Cancel();

        await _tripRepository.SaveAsync(trip, cancellationToken);
    }
}
