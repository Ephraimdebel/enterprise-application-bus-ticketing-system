using MediatR;

namespace Trip.Application.Commands.CancelTrip;

public sealed record CancelTripCommand(Guid TripId) : IRequest;
