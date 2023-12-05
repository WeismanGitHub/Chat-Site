namespace API.Endpoints.Friends.Requests.Get;

public enum FriendRequestType {
	Incoming,
	Outgoing
}

public sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
	[QueryParam]
	public FriendRequestType? Type { get; set; } = FriendRequestType.Incoming;
	[QueryParam]
	public int? Page { get; set; } = 1;
}

internal sealed class Validator : Validator<Request> {
	public Validator() {
		RuleFor(req => req.Page)
			.GreaterThanOrEqualTo(1).WithMessage("Must be greater than 1.");
	}
}
