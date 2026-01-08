
using global::Booking.Domain;
namespace Booking.Application;

public record GetBookingQuery(Guid BookingId) : IQuery<BookingResponse>;
