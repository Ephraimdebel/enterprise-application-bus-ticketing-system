using BusProvider.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusProvider.Infrastructure.Persistence.Configurations;

public class BusProviderConfiguration : IEntityTypeConfiguration<BusProviderAggregate>
{
    public void Configure(EntityTypeBuilder<BusProviderAggregate> builder)
    {
        builder.ToTable("bus_providers");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id");

        builder.Property(p => p.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(200);

        builder.OwnsOne(p => p.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(320);
        });

        builder.OwnsOne(p => p.ContactInfo, contact =>
        {
            contact.Property(c => c.PhoneNumber)
                .HasColumnName("phone_number")
                .IsRequired()
                .HasMaxLength(50);

            contact.Property(c => c.Address)
                .HasColumnName("address")
                .IsRequired()
                .HasMaxLength(500);
        });
    }
}
