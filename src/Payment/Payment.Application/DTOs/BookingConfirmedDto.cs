namespace Payment.Application.DTOs;

public record BookingConfirmedDto(
    Guid BookingId,
    decimal TotalAmount
);
