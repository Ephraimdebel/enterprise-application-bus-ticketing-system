using MediatR;
using Passenger.Application.DTOs;
using Passenger.Domain.Entities;

namespace Passenger.Application.Queries.GetPassengerById;

public sealed record GetPassengerByIdQuery(PassengerId PassengerId) : IRequest<PassengerDto?>;
