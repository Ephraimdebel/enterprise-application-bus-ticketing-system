using BusProvider.Domain.Aggregates;
using BusProvider.Domain.Repositories;
using BusProvider.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BusProvider.Infrastructure.Repositories;

public class BusRepository : IBusRepository
{
    private readonly BusProviderDbContext _dbContext;

    public BusRepository(BusProviderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AddAsync(BusAggregate bus, CancellationToken cancellationToken = default)
    {
        _dbContext.Buses.Add(bus);
        return Task.CompletedTask;
    }

    public Task<BusAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Buses.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public Task<List<BusAggregate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.Buses.AsNoTracking().ToListAsync(cancellationToken);
    }

    public Task<List<BusAggregate>> GetByProviderAsync(Guid providerId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Buses.AsNoTracking().Where(b => b.ProviderId == providerId).ToListAsync(cancellationToken);
    }

    public Task RemoveAsync(BusAggregate bus, CancellationToken cancellationToken = default)
    {
        _dbContext.Buses.Remove(bus);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsWithNumberAsync(Guid providerId, string busNumber, Guid? excludeBusId = null, CancellationToken cancellationToken = default)
    {
        return _dbContext.Buses.AnyAsync(
            b => b.ProviderId == providerId && b.BusNumber.Value == busNumber && (!excludeBusId.HasValue || b.Id != excludeBusId.Value),
            cancellationToken);
    }
}
