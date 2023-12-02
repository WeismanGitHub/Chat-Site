namespace API.Endpoints.Conversations.Get;

public sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
}

public sealed class ResConvo {
	public required string ID { get; set; }
	public required string Name { get; set; }
	public required DateTime CreatedAt { get; set; }
}
