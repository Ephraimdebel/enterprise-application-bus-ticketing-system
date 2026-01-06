using MediatR;
using Passenger.Domain.Entities;

namespace Passenger.Application.Commands.DeletePassenger;

public sealed record DeletePassengerCommand(PassengerId PassengerId) : IRequest<Unit>;
