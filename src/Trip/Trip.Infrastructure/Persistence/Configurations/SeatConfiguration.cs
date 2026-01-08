using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trip.Domain.Entities;

namespace Trip.Infrastructure.Persistence.Configurations;

public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.ToTable("Seats");

        builder.HasKey(s => s.SeatId);

        builder.Property(s => s.IsAvailable)
               .IsRequired();

        builder.OwnsOne(s => s.SeatNumber, sn =>
        {
            sn.Property(p => p.Number)
              .HasColumnName("SeatNumber")
              .HasMaxLength(10)
              .IsRequired();
        });
    }
}

