using MediatR;
using Trip.Application.DTOs;

namespace Trip.Application.Queries.GetTripSeats;

public sealed record GetTripSeatsQuery(
    Guid TripId
) : IRequest<IReadOnlyList<SeatDto>>;
