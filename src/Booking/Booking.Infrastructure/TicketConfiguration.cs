using Microsoft.EntityFrameworkCore;
using Booking.Application;
using global::Booking.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Infrastructure;

internal sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");

        builder.HasKey(ticket => ticket.Id);

        builder.OwnsOne(ticket => ticket.SeatNumber);
    }
}
