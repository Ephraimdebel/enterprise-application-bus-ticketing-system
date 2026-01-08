using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Passenger.Domain.Events;
using Passenger.Infrastructure.Persistence.OutboxEntity;
using Passenger.Infrastructure.Persistence.EF_Configurations;

namespace Passenger.Infrastructure.Persistence.DbContext;

public sealed class PassengerDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public PassengerDbContext(DbContextOptions<PassengerDbContext> options) : base(options) { }

    public DbSet<Passenger.Domain.Aggregates.Passenger> Passengers => Set<Passenger.Domain.Aggregates.Passenger>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PassengerConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddDomainEventsToOutbox();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void AddDomainEventsToOutbox()
    {
        var domainEvents = ChangeTracker
            .Entries<IHasDomainEvents>()
            .SelectMany(entry => entry.Entity.DequeueDomainEvents())
            .ToArray();

        if (domainEvents.Length == 0) return;

        var utcNow = DateTime.UtcNow;

        foreach (var domainEvent in domainEvents)
        {
            var type = domainEvent.GetType().FullName ?? domainEvent.GetType().Name;

            // store payload as JSON; type is persisted separately for deserialization and routing
            var content = JsonSerializer.Serialize(domainEvent, domainEvent.GetType(), JsonSerializerOptionsProvider.Options);

            OutboxMessages.Add(new OutboxMessage(
                id: Guid.NewGuid(),
                occurredOnUtc: domainEvent.OccurredOnUtc,
                type: type,
                content: content
            ));
        }
    }
}

internal static class JsonSerializerOptionsProvider
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };
}
