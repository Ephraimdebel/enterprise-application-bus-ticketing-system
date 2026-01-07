
using global::Booking.Domain;
namespace Booking.Application;

public record ConfirmBookingCommand(Guid BookingId) : ICommand;
