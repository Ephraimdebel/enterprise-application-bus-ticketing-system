using Microsoft.EntityFrameworkCore;
using Booking.Application;
using global::Booking.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Infrastructure;

internal sealed class BookingConfiguration : IEntityTypeConfiguration<global::Booking.Domain.Booking>
{
    public void Configure(EntityTypeBuilder<global::Booking.Domain.Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(booking => booking.Id);

        builder.OwnsOne(booking => booking.TotalPrice, priceBuilder =>
        {
            priceBuilder.Property(m => m.Amount).HasColumnType("decimal(18,2)");
        });

        builder.OwnsOne(booking => booking.TravelDate);

        builder.HasMany(booking => booking.Tickets)
            .WithOne()
            .HasForeignKey(ticket => ticket.BookingId);

        builder.Property(booking => booking.Status)
            .HasConversion<int>();
            
        builder.Property<uint>("Version").HasColumnName("xmin").HasColumnType("xid").ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();
    }
}
