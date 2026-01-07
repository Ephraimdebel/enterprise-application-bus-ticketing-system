using Microsoft.EntityFrameworkCore;
using Booking.Application;
using global::Booking.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Infrastructure;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");

        builder.HasKey(outboxMessage => outboxMessage.Id);

        builder.Property(outboxMessage => outboxMessage.Content).HasColumnType("text");
    }
}
