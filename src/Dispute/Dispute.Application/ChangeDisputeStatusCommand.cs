namespace Dispute.Application;

public sealed record ChangeDisputeStatusCommand(
    Guid DisputeId,
    string Status) : ICommand<Guid>;
