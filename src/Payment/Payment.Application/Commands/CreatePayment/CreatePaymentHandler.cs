using MediatR;
using Payment.Application.DTOs;
using Payment.Domain.Repositories;
using Payment.Domain.Entities;
using Payment.Domain.ValueObjects;
using Payment.Domain.Events;   
using Payment.Application.Interfaces;   // ✅ Add this

namespace Payment.Application.Commands.CreatePayment;

public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, PaymentDto>
{
    private readonly IPaymentRepository _repository;
    private readonly IEventPublisher _publisher; // ✅ Inject RabbitMQPublisher

    public CreatePaymentHandler(IPaymentRepository repository, IEventPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<PaymentDto> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        // 1️⃣ Create Money value object
        var money = new Money(request.Amount, request.Currency);

        // 2️⃣ Create Payment entity
        var payment = new PaymentEntity(
            request.BookingId,
            money,
            PaymentMethod.Card // or map from request
        );

        // 3️⃣ Save to DB
        await _repository.AddAsync(payment);
        await _repository.SaveChangesAsync();

        // similate 
        
        payment.MarkAsSuccess(); 

        // 4️⃣ Publish event if payment is completed
        if (payment.Status.ToString() == "Success") // or payment.Status == PaymentStatus.Completed if enum
        {
            var evt = new PaymentCompletedEvent
            {
                PaymentId = payment.Id,
                BookingId = payment.BookingId,
                Amount = payment.Amount.Amount,
                Currency = payment.Amount.Currency,
                Status = payment.Status.ToString()
            };

            _publisher.Publish(evt, "payment.exchange", "payment.completed");
        }

        // 5️⃣ Return DTO
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
