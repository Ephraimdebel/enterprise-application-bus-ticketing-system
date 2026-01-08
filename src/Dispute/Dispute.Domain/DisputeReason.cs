namespace Dispute.Domain;

public record DisputeReason
{
    public DisputeReason(string reasonCode, string description)
    {
        if (string.IsNullOrWhiteSpace(reasonCode)) throw new ArgumentException("Reason code cannot be empty.");
        
        ReasonCode = reasonCode;
        Description = description;
    }

    public string ReasonCode { get; init; }
    public string Description { get; init; }
}
