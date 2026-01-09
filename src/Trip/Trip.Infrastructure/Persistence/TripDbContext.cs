using Microsoft.EntityFrameworkCore;
using TripAggregate = Trip.Domain.Aggregates.Trip;
using Trip.Domain.Entities;

namespace Trip.Infrastructure.Persistence;

public sealed class TripDbContext : DbContext
{
    public TripDbContext(DbContextOptions<TripDbContext> options)
        : base(options) { }

    public DbSet<TripAggregate> Trips => Set<TripAggregate>();
    public DbSet<Seat> Seats => Set<Seat>();
    public DbSet<Trip.Application.OutboxMessage> OutboxMessages => Set<Trip.Application.OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TripDbContext).Assembly);
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
            .Entries<TripAggregate>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var events = entity.GetDomainEvents();
                entity.ClearDomainEvents();
                return events;
            })
            .ToList();

        var outboxMessages = domainEvents.Select(domainEvent => new Trip.Application.OutboxMessage(
            Guid.NewGuid(),
            DateTime.UtcNow,
            domainEvent.GetType().Name,
            Newtonsoft.Json.JsonConvert.SerializeObject(domainEvent, new Newtonsoft.Json.JsonSerializerSettings
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
            })))
            .ToList();

        await OutboxMessages.AddRangeAsync(outboxMessages, cancellationToken);

        return await base.SaveChangesAsync(cancellationToken);
    }
}


