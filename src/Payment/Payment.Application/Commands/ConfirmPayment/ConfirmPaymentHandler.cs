using MediatR;
using Payment.Domain.Repositories;

namespace Payment.Application.Commands.ConfirmPayment;

public class ConfirmPaymentHandler : IRequestHandler<ConfirmPaymentCommand, bool>
{
    private readonly IPaymentRepository _repository;

    public ConfirmPaymentHandler(IPaymentRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _repository.GetByIdAsync(request.PaymentId);

        if (payment is null) 
            return false;

        payment.MarkAsSuccess();

        await _repository.SaveChangesAsync();
        return true;
    }
}
