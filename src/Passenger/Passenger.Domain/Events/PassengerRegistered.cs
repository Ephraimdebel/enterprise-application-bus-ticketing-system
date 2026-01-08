using Passenger.Domain.Entities;
using Passenger.Domain.ValueObjects;

namespace Passenger.Domain.Events;

public sealed record PassengerRegistered(
    PassengerId PassengerId,
    Name Name,
    Email Email,
    PhoneNumber PhoneNumber,
    PassengerStatusCode Status,
    DateTime OccurredOnUtc
) : IDomainEvent;
