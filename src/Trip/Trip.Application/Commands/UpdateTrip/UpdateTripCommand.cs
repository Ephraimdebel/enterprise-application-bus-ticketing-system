using MediatR;
using Trip.Domain.ValueObjects;

namespace Trip.Application.Commands.UpdateTrip;

public sealed record UpdateTripCommand(
    Guid TripId,
    TravelDateTime NewDepartureTime,
    TravelDateTime NewArrivalTime
) : IRequest;
