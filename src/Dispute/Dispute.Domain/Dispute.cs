namespace Dispute.Domain;

public sealed class Dispute : Entity
{
    private readonly List<DisputeMessage> _messages = new();

    private Dispute() { } // EF Core

    public Guid BookingId { get; private set; }
    public Guid PassengerId { get; private set; }
    public DisputeStatus Status { get; private set; }
    public DisputeReason Reason { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ResolvedAt { get; private set; }

    public IReadOnlyCollection<DisputeMessage> Messages => _messages.AsReadOnly();

    public static Dispute Open(
        Guid bookingId,
        Guid passengerId,
        DisputeReason reason,
        string initialMessage)
    {
        var dispute = new Dispute
        {
            Id = Guid.NewGuid(),
            BookingId = bookingId,
            PassengerId = passengerId,
            Reason = reason,
            Status = DisputeStatus.Opened,
            CreatedAt = DateTime.UtcNow
        };

        dispute.AddMessage("Passenger", initialMessage);
        
        dispute.RaiseDomainEvent(new DisputeOpenedDomainEvent(dispute.Id));

        return dispute;
    }

    public void AddMessage(string senderRole, string messageText)
    {
        if (Status == DisputeStatus.Resolved || Status == DisputeStatus.Rejected)
        {
            throw new InvalidOperationException("Cannot add messages to a closed dispute.");
        }

        _messages.Add(DisputeMessage.Create(Id, senderRole, messageText));
    }

    public void ChangeStatus(DisputeStatus newStatus)
    {
        if (Status == DisputeStatus.Resolved || Status == DisputeStatus.Rejected)
        {
            throw new InvalidOperationException("Cannot change status of a closed dispute.");
        }

        Status = newStatus;

        if (Status == DisputeStatus.Resolved || Status == DisputeStatus.Rejected)
        {
            ResolvedAt = DateTime.UtcNow;
        }
    }
}
