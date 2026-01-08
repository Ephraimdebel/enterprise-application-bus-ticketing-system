using MediatR;
using Trip.Application.DTOs;
using Trip.Application.Interfaces;

namespace Trip.Application.Queries.GetTripSeats;

public sealed class GetTripSeatsQueryHandler
    : IRequestHandler<GetTripSeatsQuery, IReadOnlyList<SeatDto>>
{
    private readonly ITripRepository _tripRepository;

    public GetTripSeatsQueryHandler(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task<IReadOnlyList<SeatDto>> Handle(
        GetTripSeatsQuery request,
        CancellationToken cancellationToken)
    {
        var trip = await _tripRepository
            .GetByIdAsync(request.TripId, cancellationToken);

        if (trip is null)
            throw new InvalidOperationException("Trip not found.");

        return trip.Seats
            .Select(s => new SeatDto(
                s.SeatNumber.Number,
                s.IsAvailable
            ))
            .ToList();
    }
}
