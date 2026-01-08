using BusProvider.Domain.Interfaces;
using BusProvider.Infrastructure.Persistence;

namespace BusProvider.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly BusProviderDbContext _dbContext;

    public UnitOfWork(BusProviderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
