using Microsoft.EntityFrameworkCore;
using Payment.Domain.Entities;
using Payment.Infrastructure.Persistence.Configurations;

namespace Payment.Infrastructure.Persistence
{
    public class PaymentDbContext : DbContext
    {
        public DbSet<PaymentEntity> Payments { get; set; } = null!;
        public DbSet<Payment.Application.OutboxMessage> OutboxMessages { get; set; } = null!;

        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PaymentEntityConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEvents = ChangeTracker
                .Entries<PaymentEntity>()
                .Select(entry => entry.Entity)
                .SelectMany(entity =>
                {
                    var events = entity.GetDomainEvents();
                    entity.ClearDomainEvents();
                    return events;
                })
                .ToList();

            var outboxMessages = domainEvents.Select(domainEvent => new Payment.Application.OutboxMessage(
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
}
