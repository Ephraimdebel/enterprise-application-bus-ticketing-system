using Booking.Domain;

namespace Booking.Application.Interfaces
{
    public interface IOutboxService
    {
        Task AddAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
    }
}
