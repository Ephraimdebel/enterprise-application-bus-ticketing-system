using Microsoft.EntityFrameworkCore;
using Dispute.Application;
using Dispute.Domain;
using Newtonsoft.Json;

namespace Dispute.Infrastructure;

public sealed class DisputeDbContext : DbContext, IUnitOfWork
{
    public DisputeDbContext(DbContextOptions<DisputeDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Dispute> Disputes { get; set; }
    public DbSet<DisputeMessage> DisputeMessages { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DisputeDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var events = entity.GetDomainEvents();
                entity.ClearDomainEvents();
                return events;
            })
            .ToList();

        var outboxMessages = domainEvents.Select(domainEvent => new OutboxMessage(
            Guid.NewGuid(),
            DateTime.UtcNow,
            domainEvent.GetType().Name,
            JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            })))
            .ToList();

        await OutboxMessages.AddRangeAsync(outboxMessages, cancellationToken);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
