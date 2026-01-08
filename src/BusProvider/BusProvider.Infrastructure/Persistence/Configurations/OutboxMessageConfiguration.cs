using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusProvider.Infrastructure.Persistence.Configurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id).HasColumnName("id");
        builder.Property(o => o.OccurredOnUtc).HasColumnName("occurred_on_utc");
        builder.Property(o => o.ProcessedOnUtc).HasColumnName("processed_on_utc");
        builder.Property(o => o.Type).HasColumnName("type").IsRequired();
        builder.Property(o => o.Payload).HasColumnName("payload").IsRequired();
    }
}
