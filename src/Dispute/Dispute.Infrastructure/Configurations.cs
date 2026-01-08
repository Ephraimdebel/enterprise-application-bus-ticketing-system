using Dispute.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dispute.Infrastructure;

internal sealed class DisputeConfiguration : IEntityTypeConfiguration<Domain.Dispute>
{
    public void Configure(EntityTypeBuilder<Domain.Dispute> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.BookingId).IsRequired();
        builder.Property(d => d.PassengerId).IsRequired();

        builder.OwnsOne(d => d.Status, statusBuilder =>
        {
            statusBuilder.Property(s => s.Code).HasColumnName("Status").IsRequired();
        });

        builder.OwnsOne(d => d.Reason, reasonBuilder =>
        {
            reasonBuilder.Property(r => r.ReasonCode).HasColumnName("ReasonCode").IsRequired();
            reasonBuilder.Property(r => r.Description).HasColumnName("ReasonDescription");
        });

        builder.HasMany(d => d.Messages)
            .WithOne()
            .HasForeignKey(m => m.DisputeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property<uint>("Version").IsRowVersion();
    }
}

internal sealed class DisputeMessageConfiguration : IEntityTypeConfiguration<DisputeMessage>
{
    public void Configure(EntityTypeBuilder<DisputeMessage> builder)
    {
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.SenderRole).IsRequired();
        builder.Property(m => m.MessageText).IsRequired();
        builder.Property(m => m.SentAt).IsRequired();
    }
}
