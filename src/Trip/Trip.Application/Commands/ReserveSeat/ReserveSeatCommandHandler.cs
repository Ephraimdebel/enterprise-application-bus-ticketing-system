using MediatR;
using Trip.Application.Interfaces;

namespace Trip.Application.Commands.ReserveSeat;

public sealed class ReserveSeatCommandHandler
    : IRequestHandler<ReserveSeatCommand, Unit>
{
    private readonly ITripRepository _tripRepository;

    public ReserveSeatCommandHandler(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task<Unit> Handle(
        ReserveSeatCommand request,
        CancellationToken cancellationToken)
    {
        var trip = await _tripRepository.GetByIdAsync(request.TripId, cancellationToken)
            ?? throw new InvalidOperationException("Trip not found.");

        trip.ReserveSeat(request.SeatNumber);

        await _tripRepository.SaveAsync(trip, cancellationToken);

        return Unit.Value;
    }
}

