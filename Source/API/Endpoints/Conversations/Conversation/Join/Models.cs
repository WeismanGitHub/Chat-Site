namespace API.Endpoints.Conversations.SingleConvo.Join;

public sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
	public required string ConversationID { get; set; }

}
