﻿namespace API.Endpoints.ChatRooms.Create;

public sealed class Request {
	[From(Claim.AccountID, IsRequired = true)]
	public string AccountID { get; set; }
	public required string Name { get; set; }
	public required string Password { get; set; }
}

public sealed class Response {
	public required string ChatRoomID { get; set; }
}

internal sealed class Validator : Validator<Request> {
	public Validator() {
		RuleFor(req => req.Name)
			.NotEmpty()
			.MinimumLength(ChatRoom.MinNameLength)
			.WithMessage($"Chat room name must be at least {ChatRoom.MinNameLength} characters.")
			.MaximumLength(ChatRoom.MaxNameLength)
			.WithMessage($"Chat room name must be at most {ChatRoom.MaxNameLength} characters.");

		RuleFor(req => req.Password)
			.NotEmpty()
			.MinimumLength(ChatRoom.MinPasswordLength)
			.WithMessage($"Password name must be at least {ChatRoom.MinPasswordLength} characters.")
			.MaximumLength(ChatRoom.MaxPasswordLength)
			.WithMessage($"Password must be at most {ChatRoom.MaxPasswordLength} characters.");
	}
}

public sealed class Endpoint : Endpoint<Request, Response> {
    public override void Configure() {
        Post("/");
        Group<ChatRoomGroup>();
        Version(1);
        
        Summary(settings => {
            settings.Summary = "Create a chat room.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        var account = await DB.Find<User>().OneAsync(req.AccountID, cancellationToken);

        if (account == null) {
            ThrowError("Could not find your account.", 404);
        } else if (account.ChatRoomIDs.Count >= 100) {
			ThrowError("Cannot join more than 100 chat rooms.", 400);
		}

		// Not using a transaction because it didn't work for some reason.
		var chatID = ObjectId.GenerateNewId().ToString();
		await DB.InsertAsync(new ChatRoom() {
			ID = chatID,
			Name = req.Name,
			MemberIDs = new List<string>() { req.AccountID },
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password)
		}, cancellation: cancellationToken);

		account.ChatRoomIDs.Add(chatID);
		await account.SaveAsync(cancellation: cancellationToken);

		await SendAsync(new() {
			ChatRoomID = chatID
		}, cancellation: cancellationToken);
	}
}
