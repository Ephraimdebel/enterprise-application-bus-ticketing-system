using BusProvider.Domain.Aggregates;

namespace BusProvider.Domain.Interfaces;

public interface IBusProviderRepository
{
    Task AddAsync(BusProviderAggregate provider, CancellationToken cancellationToken = default);
    Task<BusProviderAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<BusProviderAggregate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task RemoveAsync(BusProviderAggregate provider, CancellationToken cancellationToken = default);
}
