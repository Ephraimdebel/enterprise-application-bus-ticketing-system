using Microsoft.EntityFrameworkCore;
using Booking.Application;
using global::Booking.Domain;

namespace Booking.Infrastructure;

internal sealed class BookingRepository : global::Booking.Domain.IBookingRepository
{
    private readonly BookingDbContext _dbContext;

    public BookingRepository(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<global::Booking.Domain.Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Bookings
            .Include(b => b.Tickets)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task AddAsync(global::Booking.Domain.Booking booking, CancellationToken cancellationToken = default)
    {
        await _dbContext.Bookings.AddAsync(booking, cancellationToken);
    }
}
