using MediatR;
using Payment.Domain.Repositories;

namespace Payment.Application.Commands.FailPayment;

public class FailPaymentHandler : IRequestHandler<FailPaymentCommand, bool>
{
    private readonly IPaymentRepository _repository;

    public FailPaymentHandler(IPaymentRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(FailPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _repository.GetByIdAsync(request.PaymentId);

        if (payment is null)
            return false;

        payment.MarkAsFailed();

        await _repository.SaveChangesAsync();
        return true;
    }
}
