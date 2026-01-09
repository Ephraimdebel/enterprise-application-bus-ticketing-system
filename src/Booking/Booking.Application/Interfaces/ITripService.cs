namespace Booking.Application.Interfaces;

public interface ITripService
{
    Task<bool> ExistsAsync(Guid tripId, CancellationToken cancellationToken = default);
}
