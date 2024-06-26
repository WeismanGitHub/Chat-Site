using Microsoft.AspNetCore.SignalR;

namespace API.Endpoints.ChatRooms.SingleChatRoom.Join;

public sealed class Request {
	[From(Claim.AccountID, IsRequired = true)]
	public string AccountID { get; set; }
	public required string ID { get; set; }

}

public sealed class Response {
	public required string Name { get; set; }
	public required DateTime CreatedAt { get; set; }
}

public sealed class Endpoint : Endpoint<Request, Response> {
	private readonly IHubContext<ChatHub> _hub;

	public override void Configure() {
		Post("/{ID}/Join");
		Group<ChatRoomGroup>();
		Version(1);

		Description(builder => builder.Accepts<Request>());
	}

	public Endpoint(IHubContext<ChatHub> hubContext) {
		_hub = hubContext;
	}

	public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
		var chat = await DB.Find<ChatRoom>()
			.MatchID(req.ID)
			.ExecuteSingleAsync(cancellationToken);

		if (chat == null) {
			ThrowError("Invalid ID", 400);
		} else if (chat.MemberIDs.Count >= 100) {
			ThrowError("Chat room is at capacity.", 400);
		} else if (chat.MemberIDs.Contains(req.AccountID)) {
			ThrowError("You are already a member of this chat room.", 400);
		}

		var transaction = new Transaction();

		chat.MemberIDs.Add(req.AccountID);
		await chat.SaveAsync(transaction.Session, cancellationToken);

		var user = await transaction.UpdateAndGet<User>()
			.MatchID(req.AccountID)
			.Modify(user => user.Push(u => u.ChatRoomIDs, chat.ID))
			.ExecuteAsync(cancellationToken);

		await transaction.CommitAsync(cancellationToken);

		await _hub.Clients.Group(chat.ID).SendAsync("UserJoined", new Member() { Id = req.AccountID, Name = user.Name }, cancellationToken: cancellationToken);
		await SendAsync(new Response() { CreatedAt = chat.CreatedAt, Name = chat.Name }, cancellation: cancellationToken);
	}
}
