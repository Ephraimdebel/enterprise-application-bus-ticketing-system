using Dispute.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dispute.Infrastructure;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Type).IsRequired();
        builder.Property(m => m.Content).IsRequired();
        builder.Property(m => m.OccurredOnUtc).IsRequired();
    }
}
