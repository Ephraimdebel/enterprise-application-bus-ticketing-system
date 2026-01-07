
using Booking.Application;
using global::Booking.Domain;
namespace Booking.Infrastructure;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
