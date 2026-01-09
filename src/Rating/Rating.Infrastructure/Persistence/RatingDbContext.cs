using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RatingEntity = Rating.Domain.Aggregates.Rating;
using Rating.Domain.ValueObjects;

namespace Rating.Infrastructure.Persistence;

public sealed class RatingDbContext : DbContext
{
    public RatingDbContext(DbContextOptions<RatingDbContext> options)
        : base(options)
    {
    }

    public DbSet<RatingEntity> Ratings => Set<RatingEntity>();
    public DbSet<Rating.Application.OutboxMessage> OutboxMessages => Set<Rating.Application.OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RatingEntity>(builder =>
        {
            builder.ToTable("Ratings");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.TripId).IsRequired();
            builder.Property(r => r.UserId).IsRequired();
            builder.Property(r => r.TargetId).IsRequired();

            var scoreConverter = new ValueConverter<Score, int>(
                v => v.Value,
                v => new Score(v)
            );

            builder.Property(r => r.Stars)
                   .HasConversion(scoreConverter)
                   .IsRequired();

            builder.Property(r => r.Comment)
                   .HasMaxLength(500);

            builder.Property(r => r.CreatedAt)
                   .IsRequired();
        });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RatingDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Note: Currently Rating aggregate doesn't have domain events, but we implement this for compliance.
        return await base.SaveChangesAsync(cancellationToken);
    }
}

