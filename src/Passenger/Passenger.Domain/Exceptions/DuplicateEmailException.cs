namespace Passenger.Domain.Exceptions;

public sealed class DuplicateEmailException : DomainException
{
    public DuplicateEmailException(string email)
        : base($"A passenger with email '{email}' already exists.") { }
}
