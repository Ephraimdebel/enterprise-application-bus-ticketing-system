using BusProvider.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusProvider.Infrastructure.Persistence.Configurations;

public class RouteConfiguration : IEntityTypeConfiguration<RouteAggregate>
{
    public void Configure(EntityTypeBuilder<RouteAggregate> builder)
    {
        builder.ToTable("routes");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id");
        builder.Property(r => r.BusId).HasColumnName("bus_id").IsRequired();

        builder.OwnsOne(r => r.Start, start =>
        {
            start.Property(p => p.Value).HasColumnName("start_location").IsRequired().HasMaxLength(200);
        });

        builder.OwnsOne(r => r.End, end =>
        {
            end.Property(p => p.Value).HasColumnName("end_location").IsRequired().HasMaxLength(200);
        });

        builder.OwnsOne(r => r.Distance, d =>
        {
            d.Property(p => p.Kilometers).HasColumnName("distance_km").IsRequired();
        });
    }
}
