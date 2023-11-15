namespace API.Endpoints.Friends.Requests.Get;

public enum FriendRequestType {
	Incoming,
	Outgoing
}

public sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
	[QueryParam]
	public FriendRequestType? FriendReqType { get; set; } = FriendRequestType.Incoming;
	[QueryParam]
	public int? Page { get; set; } = 0;
}

public sealed class Response {
	public IReadOnlyList<FriendRequest> FriendRequests { get; set; }
	public long TotalCount { get; set; }
}
