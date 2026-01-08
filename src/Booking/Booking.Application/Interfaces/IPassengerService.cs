namespace Booking.Application.Interfaces;

public interface IPassengerService
{
    Task<bool> ExistsAsync(Guid passengerId, CancellationToken cancellationToken = default);
    Task<PassengerResponse?> GetByIdAsync(Guid passengerId, CancellationToken cancellationToken = default);
}
