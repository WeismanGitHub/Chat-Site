namespace API.Endpoints.Friends.Requests.Decline;

public class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
	public required string RequestID { get; set; }
}

internal sealed class Validator : Validator<Request> {
    public Validator() {
        this.CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(req => req.RequestID)
            .NotEmpty()
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid RequestID.");
    }
}
