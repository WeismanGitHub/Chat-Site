namespace API.Endpoints.Friends.Requests.Send;

public sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
    public string RecipientID { get; set; }

    public string? Message { get; set; }
}
