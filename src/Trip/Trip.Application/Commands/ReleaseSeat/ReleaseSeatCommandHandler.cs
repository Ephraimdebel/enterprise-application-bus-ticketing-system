using MediatR;
using Trip.Application.Interfaces;

namespace Trip.Application.Commands.ReleaseSeat;

public sealed class ReleaseSeatCommandHandler
    : IRequestHandler<ReleaseSeatCommand, Unit>
{
    private readonly ITripRepository _tripRepository;

    public ReleaseSeatCommandHandler(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task<Unit> Handle(
        ReleaseSeatCommand request,
        CancellationToken cancellationToken)
    {
        var trip = await _tripRepository.GetByIdAsync(request.TripId, cancellationToken)
            ?? throw new InvalidOperationException("Trip not found.");

        trip.ReleaseSeat(request.SeatNumber);

        await _tripRepository.SaveAsync(trip, cancellationToken);

        return Unit.Value;
    }
}

