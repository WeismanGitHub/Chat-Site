namespace API.Endpoints.ChatRooms.Create;

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
        } else if (account.ChatRoomIDs.Count() >= 100) {
			ThrowError("Cannot join more than 100 chat rooms.", 400);
		}

		// Not using a transaction because it didn't work for some reason.
		var chatID = ObjectId.GenerateNewId().ToString();
		await DB.InsertAsync(new ChatRoom() {
			ID = chatID,
			Name = req.ChatRoomName,
			MemberIDs = new List<string>() { req.AccountID }
		});

		account.ChatRoomIDs.Add(chatID);
		await account.SaveAsync();

		await SendAsync(new() {
			ChatRoomID = chatID
		});
	}
}
