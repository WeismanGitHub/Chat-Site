namespace API.Endpoints.ChatRooms.Get;

public sealed class Request {
	[From(Claim.AccountID, IsRequired = true)]
	public string AccountID { get; set; }
}

public sealed class ChatRoomDTO {
	public required string ID { get; set; }
	public required string Name { get; set; }
	public required DateTime CreatedAt { get; set; }
}


public sealed class Endpoint : Endpoint<Request, List<ChatRoomDTO>> {
    public override void Configure() {
        Get("/");
        Group<ChatRoomGroup>();
        Version(1);
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        var account = await DB.Find<User>().OneAsync(req.AccountID, cancellationToken);

        if (account == null) {
            ThrowError("Could not find your account.", 404);
        }

        if (account.ChatRoomIDs.Count == 0) {
            await SendAsync(null, cancellation: cancellationToken);
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

		await SendAsync(chats, cancellation: cancellationToken);
    }
}
