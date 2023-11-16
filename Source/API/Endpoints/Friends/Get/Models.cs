namespace API.Endpoints.Friends.Get;

public sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
}

public sealed class FriendResponse {
	public string DisplayName { get; set; }
	public string ID { get; set; }
	public DateTime CreatedAt { get; set; }
}
