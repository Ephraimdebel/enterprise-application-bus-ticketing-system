
using global::Booking.Domain;
namespace Booking.Application;

public record CancelBookingCommand(Guid BookingId) : ICommand;
