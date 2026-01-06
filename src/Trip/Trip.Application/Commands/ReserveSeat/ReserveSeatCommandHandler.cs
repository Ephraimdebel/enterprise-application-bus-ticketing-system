using MediatR;
using Trip.Application.Interfaces;

namespace Trip.Application.Commands.ReserveSeat;

public sealed class ReserveSeatCommandHandler
    : IRequestHandler<ReserveSeatCommand>
{
    private readonly ITripRepository _tripRepository;

    public ReserveSeatCommandHandler(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task Handle(
        ReserveSeatCommand request,
        CancellationToken cancellationToken)
    {
        var trip = await _tripRepository
            .GetByIdAsync(request.TripId, cancellationToken);

        if (trip is null)
            throw new InvalidOperationException("Trip not found.");

        trip.ReserveSeat(request.SeatNumber);

        await _tripRepository.SaveAsync(trip, cancellationToken);
    }
}
