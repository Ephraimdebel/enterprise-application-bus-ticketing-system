namespace Dispute.Application;

public sealed record OpenDisputeCommand(
    Guid BookingId,
    Guid PassengerId,
    string ReasonCode,
    string Description,
    string InitialMessage) : ICommand<Guid>;
