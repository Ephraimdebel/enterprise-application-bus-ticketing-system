namespace Dispute.Domain;

public sealed class DisputeMessage : Entity
{
    private DisputeMessage() { } // EF Core

    private DisputeMessage(Guid disputeId, string senderRole, string messageText)
    {
        Id = Guid.NewGuid();
        DisputeId = disputeId;
        SenderRole = senderRole;
        MessageText = messageText;
        SentAt = DateTime.UtcNow;
    }

    public Guid DisputeId { get; private set; }
    public string SenderRole { get; private set; }
    public string MessageText { get; private set; }
    public DateTime SentAt { get; private set; }

    internal static DisputeMessage Create(Guid disputeId, string senderRole, string messageText)
    {
        return new DisputeMessage(disputeId, senderRole, messageText);
    }
}
