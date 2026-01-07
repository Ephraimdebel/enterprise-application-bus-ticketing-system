namespace Booking.Application;
using global::Booking.Domain;

public record BookingResponse(
    Guid Id,
    Guid PassengerId,
    Guid TripId,
    string Status,
    decimal Price,
    string Currency,
    List<string> SeatNumbers);
