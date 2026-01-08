namespace Booking.Application;
using global::Booking.Domain;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
