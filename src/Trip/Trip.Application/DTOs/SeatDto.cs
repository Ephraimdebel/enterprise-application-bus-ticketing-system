namespace Trip.Application.DTOs;

public sealed record SeatDto(
    string SeatNumber,
    bool IsAvailable
);
