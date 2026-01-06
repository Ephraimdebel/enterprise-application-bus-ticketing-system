using MediatR;
using Trip.Application.DTOs;
using Trip.Application.Interfaces;

namespace Trip.Application.Queries.GetTripById;

public sealed class GetTripByIdQueryHandler
    : IRequestHandler<GetTripByIdQuery, TripDto?>
{
    private readonly ITripRepository _tripRepository;

    public GetTripByIdQueryHandler(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task<TripDto?> Handle(
        GetTripByIdQuery request,
        CancellationToken cancellationToken)
    {
        var trip = await _tripRepository
            .GetByIdAsync(request.TripId, cancellationToken);

        if (trip is null)
            return null;

        var totalSeats = trip.Seats.Count;
        var availableSeats = trip.Seats.Count(s => s.IsAvailable);

        return new TripDto
        {
            TripId = trip.TripId,
            Status = trip.Status,

            DepartureDate = trip.DepartureTime.Date,
            DepartureTime = trip.DepartureTime.Time,

            ArrivalDate = trip.ArrivalTime.Date,
            ArrivalTime = trip.ArrivalTime.Time,

            Origin = trip.Route.Origin,
            Destination = trip.Route.Destination,

            TotalSeats = totalSeats,
            AvailableSeats = availableSeats
        };
    }
}
