using Dispute.Domain;

namespace Dispute.Application;

internal sealed class OpenDisputeCommandHandler : ICommandHandler<OpenDisputeCommand, Guid>
{
    private readonly IDisputeRepository _disputeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OpenDisputeCommandHandler(IDisputeRepository disputeRepository, IUnitOfWork unitOfWork)
    {
        _disputeRepository = disputeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(OpenDisputeCommand request, CancellationToken cancellationToken)
    {
        var reason = new DisputeReason(request.ReasonCode, request.Description);
        
        var dispute = Domain.Dispute.Open(
            request.BookingId,
            request.PassengerId,
            reason,
            request.InitialMessage);

        await _disputeRepository.AddAsync(dispute, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return dispute.Id;
    }
}
