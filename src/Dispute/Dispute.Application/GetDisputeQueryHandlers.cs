using Dispute.Domain;

namespace Dispute.Application;

internal sealed class GetDisputeQueryHandlers : 
    IQueryHandler<GetDisputeByIdQuery, DisputeResponse?>,
    IQueryHandler<GetUserDisputesQuery, IEnumerable<DisputeResponse>>
{
    private readonly IDisputeRepository _disputeRepository;

    public GetDisputeQueryHandlers(IDisputeRepository disputeRepository)
    {
        _disputeRepository = disputeRepository;
    }

    public async Task<DisputeResponse?> Handle(GetDisputeByIdQuery request, CancellationToken cancellationToken)
    {
        var dispute = await _disputeRepository.GetByIdAsync(request.DisputeId, cancellationToken);

        if (dispute is null) return null;

        return Map(dispute);
    }

    public async Task<IEnumerable<DisputeResponse>> Handle(GetUserDisputesQuery request, CancellationToken cancellationToken)
    {
        var disputes = await _disputeRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        return disputes.Select(Map);
    }

    private static DisputeResponse Map(Domain.Dispute dispute)
    {
        return new DisputeResponse(
            dispute.Id,
            dispute.BookingId,
            dispute.PassengerId,
            dispute.Status.Code,
            dispute.Reason.ReasonCode,
            dispute.Reason.Description,
            dispute.CreatedAt,
            dispute.ResolvedAt);
    }
}
