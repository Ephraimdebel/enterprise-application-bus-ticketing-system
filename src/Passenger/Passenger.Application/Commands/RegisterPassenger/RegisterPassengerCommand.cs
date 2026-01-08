using MediatR;
using Passenger.Application.DTOs;
using Passenger.Domain.Entities;

namespace Passenger.Application.Commands.RegisterPassenger;

public sealed record RegisterPassengerCommand(
    PassengerId PassengerId,
    string FirstName,
    string LastName,
    string Email,
    string CountryCode,
    string PhoneNumber
) : IRequest<PassengerDto>;
