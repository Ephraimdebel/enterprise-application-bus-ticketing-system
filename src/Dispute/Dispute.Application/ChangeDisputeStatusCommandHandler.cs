using Dispute.Domain;

namespace Dispute.Application;

internal sealed class ChangeDisputeStatusCommandHandler : ICommandHandler<ChangeDisputeStatusCommand, Guid>
{
    private readonly IDisputeRepository _disputeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeDisputeStatusCommandHandler(IDisputeRepository disputeRepository, IUnitOfWork unitOfWork)
    {
        _disputeRepository = disputeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(ChangeDisputeStatusCommand request, CancellationToken cancellationToken)
    {
        var dispute = await _disputeRepository.GetByIdAsync(request.DisputeId, cancellationToken);

        if (dispute is null)
        {
            throw new InvalidOperationException("Dispute not found.");
        }

        var newStatus = DisputeStatus.FromCode(request.Status);
        dispute.ChangeStatus(newStatus);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return dispute.Id;
    }
}
