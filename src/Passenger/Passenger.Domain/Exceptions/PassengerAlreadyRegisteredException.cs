using Passenger.Domain.Entities;

namespace Passenger.Domain.Exceptions;

public sealed class PassengerAlreadyRegisteredException : DomainException
{
    public PassengerAlreadyRegisteredException(PassengerId id)
        : base($"Passenger '{id}' is already registered.") { }
}
