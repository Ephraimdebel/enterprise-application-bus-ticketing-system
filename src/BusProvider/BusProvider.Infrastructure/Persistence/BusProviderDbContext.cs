using BusProvider.Domain.Abstractions;
using BusProvider.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace BusProvider.Infrastructure.Persistence;

public class BusProviderDbContext : DbContext
{
    public BusProviderDbContext(DbContextOptions<BusProviderDbContext> options) : base(options)
    {
    }

    public DbSet<BusProviderAggregate> Providers => Set<BusProviderAggregate>();
    public DbSet<BusAggregate> Buses => Set<BusAggregate>();
    public DbSet<RouteAggregate> Routes => Set<RouteAggregate>();
    public DbSet<ScheduleAggregate> Schedules => Set<ScheduleAggregate>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
            .Entries<IHasDomainEvents>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        if (domainEvents.Count > 0)
        {
            var outboxMessages = domainEvents.Select(OutboxMessage.FromDomainEvent).ToList();
            OutboxMessages.AddRange(outboxMessages);
            result += await base.SaveChangesAsync(cancellationToken);
        }

        foreach (var entry in ChangeTracker.Entries<IHasDomainEvents>())
        {
            entry.Entity.ClearDomainEvents();
        }

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<DomainEvent>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BusProviderDbContext).Assembly);
    }
}
