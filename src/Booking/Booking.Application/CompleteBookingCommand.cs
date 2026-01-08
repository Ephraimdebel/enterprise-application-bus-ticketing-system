using MediatR;

namespace Booking.Application;

public record CompleteBookingCommand(Guid BookingId) : ICommand<Guid>;
