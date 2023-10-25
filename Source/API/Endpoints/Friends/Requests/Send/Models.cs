namespace API.Endpoints.Friends.Requests.Send;

public sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
    public string RecipientID { get; set; }

    public string? Message { get; set; }
}

internal sealed class Validator : Validator<Request> {
    public Validator() {
        this.CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(req => req.RecipientID)
            .NotEmpty()
            .Equal(req => req.AccountID)
            .WithMessage("You cannot befriend yourself.")
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("Invalid RecipientID.")
            .MustAsync((id, _) => DB.Find<User>().MatchID(id).ExecuteAnyAsync())
            .WithMessage("Recipient does not exist.");

        RuleFor(req => req.Message)
            .MinimumLength(1)
            .MaximumLength(250);

        RuleFor(req => req)
            .Must(req => ObjectId.TryParse(req.RecipientID, out _))
            .WithMessage("Invalid RecipientID.")
            .MustAsync(async (req, _) => {
                var exists = await DB.Find<FriendRequest>()
                     .Match(fr => fr.RecipientID == req.RecipientID)
                     .Match(fr => fr.RequesterID == req.AccountID)
                     .ExecuteAnyAsync();
                return !exists;
            })
            .WithMessage("You've already sent this user a friend request.");
    }
}
