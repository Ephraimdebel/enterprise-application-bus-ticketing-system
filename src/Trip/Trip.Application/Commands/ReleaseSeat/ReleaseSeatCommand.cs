using MediatR;
using Trip.Domain.ValueObjects;

namespace Trip.Application.Commands.ReleaseSeat;

public sealed record ReleaseSeatCommand(
    Guid TripId,
    SeatNumber SeatNumber
) : IRequest;
