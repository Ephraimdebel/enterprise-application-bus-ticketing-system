using Microsoft.EntityFrameworkCore;
using Booking.Application;
using global::Booking.Domain;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;

namespace Booking.Infrastructure;

public sealed class BookingDbContext : DbContext, IUnitOfWork
{
    public BookingDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<global::Booking.Domain.Booking> Bookings { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingDbContext).Assembly);
        
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
