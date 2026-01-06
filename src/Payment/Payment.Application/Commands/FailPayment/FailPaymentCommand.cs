using MediatR;

namespace Payment.Application.Commands.FailPayment;

public record FailPaymentCommand(Guid PaymentId) : IRequest<bool>;
