namespace API.Endpoints.ChatRooms.Get;

public sealed class Endpoint : Endpoint<Request, List<ChatRoomDTO>> {
    public override void Configure() {
        Get("/");
        Group<ChatRoomGroup>();
        Version(1);
        
        Summary(settings => {
            settings.Summary = "Get logged in account's chat rooms.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        var account = await DB.Find<User>()
            .Project(u => new() { ChatRoomIDs = u.ChatRoomIDs })
            .OneAsync(req.AccountID);

        if (account == null) {
            ThrowError("Could not find your account.", 404);
        }

        if (account.ChatRoomIDs.Count == 0) {
            await SendAsync(null);
        }

        var chats = await DB
			.Find<ChatRoom, ChatRoomDTO>()
            .Match(c => account.ChatRoomIDs.Contains(c.ID))
            .Project(u => new() {
				ID = u.ID,
				Name = u.Name,
				CreatedAt = DateTime.UtcNow,
            })
            .ExecuteAsync(cancellationToken);

		await SendAsync(chats);
    }
}
