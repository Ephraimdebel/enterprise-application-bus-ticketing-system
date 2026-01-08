namespace Booking.Application.Interfaces;

public interface IPassengerService
{
    Task<bool> ExistsAsync(Guid passengerId, CancellationToken cancellationToken = default);
}
