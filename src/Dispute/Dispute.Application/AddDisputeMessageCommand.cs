namespace Dispute.Application;

public sealed record AddDisputeMessageCommand(
    Guid DisputeId,
    string SenderRole,
    string MessageText) : ICommand<Guid>;
