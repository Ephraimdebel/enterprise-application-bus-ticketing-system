using Dispute.Domain;

namespace Dispute.Application;

internal sealed class AddDisputeMessageCommandHandler : ICommandHandler<AddDisputeMessageCommand, Guid>
{
    private readonly IDisputeRepository _disputeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddDisputeMessageCommandHandler(IDisputeRepository disputeRepository, IUnitOfWork unitOfWork)
    {
        _disputeRepository = disputeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(AddDisputeMessageCommand request, CancellationToken cancellationToken)
    {
        var dispute = await _disputeRepository.GetByIdAsync(request.DisputeId, cancellationToken);

        if (dispute is null)
        {
            throw new InvalidOperationException("Dispute not found.");
        }

        dispute.AddMessage(request.SenderRole, request.MessageText);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return dispute.Id;
    }
}
