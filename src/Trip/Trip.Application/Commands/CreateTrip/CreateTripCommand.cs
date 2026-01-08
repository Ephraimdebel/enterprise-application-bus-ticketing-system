using MediatR;
using Trip.Domain.ValueObjects;

namespace Trip.Application.Commands.CreateTrip;

public sealed record CreateTripCommand(
    Guid TripId,
    Guid BusId,
    Guid RouteId,
    TravelDateTime DepartureTime,
    TravelDateTime ArrivalTime,
    int SeatCapacity, // placeholder for now 
    TripPrice Price 
) : IRequest;
