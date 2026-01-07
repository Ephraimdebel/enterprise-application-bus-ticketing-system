using BusProvider.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusProvider.Infrastructure.Persistence.Configurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<ScheduleAggregate>
{
    public void Configure(EntityTypeBuilder<ScheduleAggregate> builder)
    {
        builder.ToTable("schedules");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.BusId).HasColumnName("bus_id").IsRequired();
        builder.Property(s => s.RouteId).HasColumnName("route_id").IsRequired();

        builder.OwnsOne(s => s.TripDate, td =>
        {
            td.Property(p => p.Value).HasColumnName("trip_date").IsRequired();
        });

        builder.OwnsOne(s => s.Departure, dep =>
        {
            dep.Property(p => p.Value).HasColumnName("departure_time").IsRequired();
        });

        builder.OwnsOne(s => s.Arrival, arr =>
        {
            arr.Property(p => p.Value).HasColumnName("arrival_time").IsRequired();
        });

        builder.OwnsOne(s => s.SeatsAvailable, sa =>
        {
            sa.Property(p => p.Value).HasColumnName("seats_available").IsRequired();
        });
    }
}
