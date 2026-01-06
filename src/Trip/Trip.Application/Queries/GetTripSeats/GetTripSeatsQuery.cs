using MediatR;

namespace Trip.Application.Queries.GetTripSeats;

public sealed record GetTripSeatsQuery(
    Guid TripId
) : IRequest<IReadOnlyList<SeatDto>>;
