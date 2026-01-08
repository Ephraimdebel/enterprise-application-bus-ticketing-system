using MediatR;
using Passenger.Application.DTOs;
using Passenger.Domain.Entities;

namespace Passenger.Application.Commands.UpdatePassenger;

public sealed record UpdatePassengerCommand(
    PassengerId PassengerId,
    string FirstName,
    string LastName,
    string Email,
    string CountryCode,
    string PhoneNumber
) : IRequest<PassengerDto>;
