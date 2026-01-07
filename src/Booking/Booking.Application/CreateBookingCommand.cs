
using global::Booking.Domain;
namespace Booking.Application;

public record CreateBookingCommand(
    Guid PassengerId,
    Guid TripId,
    DateOnly TravelDate,
    decimal TotalAmount,
    string Currency,
    List<string> SeatNumbers) : ICommand;
