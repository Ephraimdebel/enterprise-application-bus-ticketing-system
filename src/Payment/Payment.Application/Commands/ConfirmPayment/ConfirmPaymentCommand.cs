using MediatR;

namespace Payment.Application.Commands.ConfirmPayment;

public record ConfirmPaymentCommand(Guid PaymentId) : IRequest<bool>;
