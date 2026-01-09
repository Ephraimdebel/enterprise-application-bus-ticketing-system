using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TripAggregate = Trip.Domain.Aggregates.Trip;

namespace Trip.Infrastructure.Persistence.Configurations;

public class TripConfiguration : IEntityTypeConfiguration<TripAggregate>
{
       public void Configure(EntityTypeBuilder<TripAggregate> builder)
       {
              builder.ToTable("Trips");

              builder.HasKey(t => t.TripId);

              builder.Property(t => t.Status)
                     .IsRequired();

              builder.Property(t => t.BusId)
                     .IsRequired();

              builder.Property(t => t.RouteId)
                     .IsRequired();

              builder.OwnsOne(t => t.DepartureTime, dt =>
              {
                     dt.Property(p => p.Date)
                 .HasColumnName("DepartureDate")
                 .IsRequired();

                     dt.Property(p => p.Time)
                 .HasColumnName("DepartureTime")
                 .IsRequired();
              });

              builder.OwnsOne(t => t.ArrivalTime, dt =>
              {
                     dt.Property(p => p.Date)
                 .HasColumnName("ArrivalDate")
                 .IsRequired();

                     dt.Property(p => p.Time)
                 .HasColumnName("ArrivalTime")
                 .IsRequired();
              });

              builder.HasMany(t => t.Seats)
                     .WithOne()
                     .HasForeignKey(s => s.TripId)
                     .IsRequired()
                     .OnDelete(DeleteBehavior.Cascade);

              builder.Metadata
                     .FindNavigation(nameof(TripAggregate.Seats))!
                     .SetPropertyAccessMode(PropertyAccessMode.Field);
              builder.OwnsOne(t => t.Price, p =>
                 {
                        p.Property(pv => pv.Amount)
              .HasColumnName("price")
              .IsRequired();
                 });

        }
}

