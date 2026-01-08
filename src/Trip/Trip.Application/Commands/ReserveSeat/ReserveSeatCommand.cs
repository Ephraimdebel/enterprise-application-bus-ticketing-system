using MediatR;
using Trip.Domain.ValueObjects;

namespace Trip.Application.Commands.ReserveSeat;

public sealed record ReserveSeatCommand(
    Guid TripId,
    SeatNumber SeatNumber
) : IRequest;
