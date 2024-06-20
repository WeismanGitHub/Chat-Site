namespace API.Endpoints.ChatRooms.Create;

public sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
	public string ChatRoomName { get; set; }
}

public sealed class Response {
	public string ChatRoomID { get; set; }
}

internal sealed class Validator : Validator<Request> {
	public Validator() {
		RuleFor(req => req.ChatRoomName)
			.NotEmpty()
			.MinimumLength(ChatRoom.MinNameLength)
			.WithMessage($"Chat room name must be at least {ChatRoom.MinNameLength} characters.")
			.MaximumLength(ChatRoom.MaxNameLength)
			.WithMessage($"Chat room name must be at most {ChatRoom.MaxNameLength} characters.");
	}
}
