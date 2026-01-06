namespace Passenger.Application.Abstractions;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
