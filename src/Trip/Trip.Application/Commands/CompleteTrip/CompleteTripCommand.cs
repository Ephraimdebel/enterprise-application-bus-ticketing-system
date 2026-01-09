using MediatR;

namespace Trip.Application.Commands.CompleteTrip;

public sealed record CompleteTripCommand(Guid TripId) : IRequest<Unit>;
