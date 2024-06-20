namespace API.Endpoints.ChatRooms.SingleChatRoom.Get;

public sealed class Request {
	[From(Claim.AccountID, IsRequired = true)]
	public string AccountID { get; set; }
	public required string ChatRoomID { get; set; }

}

public sealed class Member {
	public required string ID { get; set; }
	public required string Name { get; set; }
}

public sealed class Response {
	public required string ID { get; set; }
	public required string Name { get; set; }
	public DateTime CreatedAt { get; set; }
	public required List<Member> Members { get; set; }
}

public sealed class Endpoint : Endpoint<Request, Response> {
    public override void Configure() {
		Get("/{ChatRoomID}");
		Group<ChatRoomGroup>();
        Version(1);
        
        Summary(settings => {
            settings.Summary = "Get the data for a chat room.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        var chat = await DB
		.Find<ChatRoom>()
			.MatchID(req.ChatRoomID)
            .Match(c => c.MemberIDs.Contains(req.AccountID))
            .Project(c => new() {
				ID = c.ID,
				Name = c.Name,
				MemberIDs = c.MemberIDs,
				CreatedAt = c.CreatedAt
            })
            .ExecuteSingleAsync();

		if (chat == null) {
			ThrowError("Could not find chat room.", 404);
		}

		var members = await DB.Find<User, Member>()
			.Match(u => chat.MemberIDs.Contains(u.ID))
			.Project(u => new() {
				ID = u.ID,
				Name = u.DisplayName,
			})
			.ExecuteAsync(cancellationToken);

		await SendAsync(new Response() {
			ID = chat.ID,
			Name = chat.Name,
			CreatedAt = chat.CreatedAt,
			Members = members
		}, cancellation: cancellationToken);
    }
}
