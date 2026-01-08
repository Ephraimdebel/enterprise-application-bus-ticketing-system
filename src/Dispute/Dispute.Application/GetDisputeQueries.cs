namespace Dispute.Application;

public sealed record GetDisputeByIdQuery(Guid DisputeId) : IQuery<DisputeResponse?>;

public sealed record GetUserDisputesQuery(Guid UserId) : IQuery<IEnumerable<DisputeResponse>>;
