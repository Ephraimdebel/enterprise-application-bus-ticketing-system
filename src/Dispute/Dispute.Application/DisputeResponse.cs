namespace Dispute.Application;

public sealed record DisputeResponse(
    Guid Id,
    Guid BookingId,
    Guid PassengerId,
    string Status,
    string ReasonCode,
    string Description,
    DateTime CreatedAt,
    DateTime? ResolvedAt);

public sealed record DisputeMessageResponse(
    Guid Id,
    string SenderRole,
    string MessageText,
    DateTime SentAt);
