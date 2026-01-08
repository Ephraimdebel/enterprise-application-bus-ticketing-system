using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Passenger.Infrastructure.Persistence.OutboxEntity;

namespace Passenger.Infrastructure.Persistence.EF_Configurations;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type).HasColumnName("type").HasMaxLength(500).IsRequired();
        builder.Property(x => x.Content).HasColumnName("content").IsRequired();
        builder.Property(x => x.OccurredOnUtc).HasColumnName("occurred_on_utc").IsRequired();

        builder.Property(x => x.Attempts).HasColumnName("attempts").IsRequired();
        builder.Property(x => x.LastAttemptOnUtc).HasColumnName("last_attempt_on_utc");
        builder.Property(x => x.LastError).HasColumnName("last_error");
        builder.Property(x => x.ProcessedOnUtc).HasColumnName("processed_on_utc");

        builder.HasIndex(x => x.ProcessedOnUtc);
        builder.HasIndex(x => x.OccurredOnUtc);
    }
}
