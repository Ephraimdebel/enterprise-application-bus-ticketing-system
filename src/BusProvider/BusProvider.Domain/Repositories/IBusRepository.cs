using BusProvider.Domain.Aggregates;

namespace BusProvider.Domain.Repositories;

public interface IBusRepository
{
    Task AddAsync(BusAggregate bus, CancellationToken cancellationToken = default);
    Task<BusAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<BusAggregate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<BusAggregate>> GetByProviderAsync(Guid providerId, CancellationToken cancellationToken = default);
    Task RemoveAsync(BusAggregate bus, CancellationToken cancellationToken = default);
    Task<bool> ExistsWithNumberAsync(Guid providerId, string busNumber, Guid? excludeBusId = null, CancellationToken cancellationToken = default);
}
