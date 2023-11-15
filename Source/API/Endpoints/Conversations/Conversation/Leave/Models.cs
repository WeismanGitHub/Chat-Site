namespace API.Endpoints.Conversations.SingleConvo.Leave;

public sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
	public required string ConversationID { get; set; }

}
