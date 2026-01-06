using MediatR;
using Payment.Application.DTOs;
using Payment.Domain.Repositories;
using Payment.Domain.Entities;
using Payment.Domain.ValueObjects;

namespace Payment.Application.Commands.CreatePayment;

public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, PaymentDto>
{
    private readonly IPaymentRepository _repository;

    public CreatePaymentHandler(IPaymentRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaymentDto> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
{
    var money = new Money(request.Amount, request.Currency);

    var payment = new PaymentEntity(
        request.BookingId,
        money,
        PaymentMethod.Card   // or map from request
    );

    await _repository.AddAsync(payment);
    await _repository.SaveChangesAsync();

    return new PaymentDto
    {
        Id = payment.Id,
        BookingId = payment.BookingId,
        Amount = payment.Amount.Amount,
        Currency = payment.Amount.Currency,
        Status = payment.Status.ToString()
    };
}

}
