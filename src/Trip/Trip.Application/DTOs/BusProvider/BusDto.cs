namespace Trip.Application.DTOs;

public sealed record BusDto(Guid Id, Guid ProviderId, string BusNumber, string BusType, int SeatCapacity);
