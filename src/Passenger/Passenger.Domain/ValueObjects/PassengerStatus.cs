using Passenger.Domain.Exceptions;

namespace Passenger.Domain.ValueObjects;

public sealed class PassengerStatus : ValueObject
{
    public static PassengerStatus Active { get; } = new(PassengerStatusCode.Active);
    public static PassengerStatus Suspended { get; } = new(PassengerStatusCode.Suspended);
    public static PassengerStatus Deleted { get; } = new(PassengerStatusCode.Deleted);

    public PassengerStatusCode Code { get; }

    private PassengerStatus(PassengerStatusCode code) => Code = code;

    public static PassengerStatus From(PassengerStatusCode code) => code switch
    {
        PassengerStatusCode.Active => Active,
        PassengerStatusCode.Suspended => Suspended,
        PassengerStatusCode.Deleted => Deleted,
        _ => throw new ValidationException("Unknown passenger status.")
    };

    public bool IsDeleted => Code == PassengerStatusCode.Deleted;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Code;
    }

    public override string ToString() => Code.ToString();
}

public enum PassengerStatusCode
{
    Active = 1,
    Suspended = 2,
    Deleted = 3
}
