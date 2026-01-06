using Passenger.Application.DTOs;
using Passenger.Domain.Aggregates;

namespace Passenger.Application.Mappings;

public static class PassengerMappings
{
    public static PassengerDto ToDto(this Passenger.Domain.Aggregates.Passenger passenger)
        => new(
            Id: passenger.Id.Value,
            FirstName: passenger.Name.FirstName,
            LastName: passenger.Name.LastName,
            Email: passenger.Email.Value,
            CountryCode: passenger.PhoneNumber.CountryCode,
            PhoneNumber: passenger.PhoneNumber.Number,
            Status: passenger.Status.Code.ToString(),
            CreatedAtUtc: passenger.CreatedAtUtc,
            UpdatedAtUtc: passenger.UpdatedAtUtc
        );
}
