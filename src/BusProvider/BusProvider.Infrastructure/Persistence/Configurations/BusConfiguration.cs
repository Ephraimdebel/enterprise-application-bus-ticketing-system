using BusProvider.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusProvider.Infrastructure.Persistence.Configurations;

public class BusConfiguration : IEntityTypeConfiguration<BusAggregate>
{
    public void Configure(EntityTypeBuilder<BusAggregate> builder)
    {
        builder.ToTable("buses");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("id");
        builder.Property(b => b.ProviderId).HasColumnName("provider_id").IsRequired();

        builder.OwnsOne(b => b.BusNumber, bn =>
        {
            bn.Property(p => p.Value).HasColumnName("bus_number").IsRequired().HasMaxLength(32);
        });

        builder.OwnsOne(b => b.BusType, bt =>
        {
            bt.Property(p => p.Value).HasColumnName("bus_type").IsRequired().HasMaxLength(64);
        });

        builder.OwnsOne(b => b.SeatCapacity, sc =>
        {
            sc.Property(p => p.Value).HasColumnName("seat_capacity").IsRequired();
        });

    }
}
