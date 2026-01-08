using MediatR;
using Trip.Application.DTOs;

namespace Trip.Application.Queries.GetTripById;

public sealed record GetTripByIdQuery(Guid TripId)
    : IRequest<TripDto?>;
