using Passenger.Domain.Entities;

namespace Passenger.Domain.Exceptions;

public sealed class PassengerNotFoundException : DomainException
{
    public PassengerNotFoundException(PassengerId id)
        : base($"Passenger '{id}' was not found.") { }
}
