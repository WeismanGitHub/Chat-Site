namespace API.Endpoints.Friends.Remove;

public sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
	public string FriendID { get; set; }
}

internal sealed class Validator : Validator<Request> {
	public Validator() {
		RuleFor(req => req.FriendID)
			.NotEmpty()
			.Must(id => ObjectId.TryParse(id, out _))
			.WithMessage("Invalid FriendID.");
	}
}
