namespace Passenger.Application.DTOs;

public sealed record PassengerDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string CountryCode,
    string PhoneNumber,
    string Status,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);
