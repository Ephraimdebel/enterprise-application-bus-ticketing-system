using Microsoft.EntityFrameworkCore;
using Notification.Domain.Entities;

namespace Notification.Infrastructure.Persistence
{
    public class NotificationDbContext : DbContext
    {
        public DbSet<NotificationEntity> Notifications { get; set; }

        public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationEntity>().HasKey(n => n.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
