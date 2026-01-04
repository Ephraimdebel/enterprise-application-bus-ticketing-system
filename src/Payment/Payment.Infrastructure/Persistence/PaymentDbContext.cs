using Microsoft.EntityFrameworkCore;
using Payment.Domain.Entities;
using Payment.Infrastructure.Persistence.Configurations;

namespace Payment.Infrastructure.Persistence
{
    public class PaymentDbContext : DbContext
    {
        public DbSet<PaymentEntity> Payments { get; set; }

        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PaymentEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
