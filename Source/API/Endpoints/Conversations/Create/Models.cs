namespace API.Endpoints.Conversations.Create;

public sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
}
