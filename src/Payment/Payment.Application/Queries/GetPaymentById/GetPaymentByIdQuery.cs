using MediatR;
using Payment.Application.DTOs;

namespace Payment.Application.Queries.GetPaymentById;

public record GetPaymentByIdQuery(Guid PaymentId) : IRequest<PaymentDto>;
