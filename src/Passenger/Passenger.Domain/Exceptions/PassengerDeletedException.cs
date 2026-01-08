namespace Passenger.Domain.Exceptions;

public sealed class PassengerDeletedException : DomainException
{
    public PassengerDeletedException() : base("Passenger is deleted and cannot be modified.") { }
}
