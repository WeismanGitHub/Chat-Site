namespace API.Endpoints.Conversations.Create;

public sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
	public string ConversationName { get; set; }
}

public sealed class Response {
	public string ConversationID { get; set; }
}

internal sealed class Validator : Validator<Request> {
	public Validator() {
		RuleFor(req => req.ConversationName)
			.NotEmpty()
			.MinimumLength(Conversation.MinNameLength)
			.WithMessage($"Conversation Name must be at least {Conversation.MinNameLength} characters.")
			.MaximumLength(Conversation.MaxNameLength)
			.WithMessage($"Conversation Name must be at most {Conversation.MaxNameLength} characters.");
	}
}
