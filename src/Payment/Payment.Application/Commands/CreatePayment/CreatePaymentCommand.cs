using MediatR;
using Payment.Application.DTOs;

namespace Payment.Application.Commands.CreatePayment;

public record CreatePaymentCommand(
    Guid BookingId,
    decimal Amount,
    string Currency
) : IRequest<PaymentDto>;
