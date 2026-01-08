namespace Booking.Application.Interfaces;

public sealed record PassengerResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string CountryCode,
    string PhoneNumber);
