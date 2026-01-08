namespace Dispute.Domain;

public record DisputeStatus
{
    public static readonly DisputeStatus Opened = new("Opened");
    public static readonly DisputeStatus InReview = new("InReview");
    public static readonly DisputeStatus Resolved = new("Resolved");
    public static readonly DisputeStatus Rejected = new("Rejected");

    private DisputeStatus(string code)
    {
        Code = code;
    }

    public string Code { get; init; }

    public static DisputeStatus FromCode(string code) => code switch
    {
        "Opened" => Opened,
        "InReview" => InReview,
        "Resolved" => Resolved,
        "Rejected" => Rejected,
        _ => throw new ArgumentException("Invalid status code", nameof(code))
    };
}
