using MediatR;
using Trip.Application.Interfaces;

namespace Trip.Application.Commands.CompleteTrip;

public sealed class CompleteTripCommandHandler : IRequestHandler<CompleteTripCommand, Unit>
{
    private readonly ITripRepository _tripRepository;

    public CompleteTripCommandHandler(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task<Unit> Handle(CompleteTripCommand request, CancellationToken cancellationToken)
    {
        var trip = await _tripRepository.GetByIdAsync(request.TripId, cancellationToken)
            ?? throw new InvalidOperationException("Trip not found.");

        trip.Complete();

        await _tripRepository.SaveAsync(trip, cancellationToken);

        return Unit.Value;
    }
}
