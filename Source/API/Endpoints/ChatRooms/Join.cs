namespace API.Endpoints.ChatRooms.Join;

public sealed class Request {
	[From(Claim.AccountID, IsRequired = true)]
	public string AccountID { get; set; }
	public required string Name { get; set; }
	public required string Password { get; set; }

}

public sealed class Endpoint : Endpoint<Request> {
	public override void Configure() {
		Post("/Join");
		Group<ChatRoomGroup>();
		Version(1);

		Description(builder => builder.Accepts<Request>());
	}

	public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
		var chat = await DB.Find<ChatRoom>()
			.Match(c => c.Name == req.Name)
			.ExecuteSingleAsync(cancellationToken);

		if (chat == null) {
			ThrowError("Invalid Credentials", 400);
		} else if (chat.MemberIDs.Count >= 100) {
			ThrowError("Chat room is at capacity.", 400);
		} else if (chat.MemberIDs.Contains(req.AccountID)) {
			ThrowError("You are already a member of this chat room.", 400);
		} else if (BCrypt.Net.BCrypt.Verify(req.Password, chat.PasswordHash)) {
			ThrowError("Invalid Credentials", 400);
		}

		var transaction = new Transaction();

		chat.MemberIDs.Add(req.AccountID);
		await chat.SaveAsync(transaction.Session, cancellationToken);

		await transaction.Update<User>()
			.MatchID(req.AccountID)
			.Modify(user => user.Push(u => u.ChatRoomIDs, chat.ID))
			.ExecuteAsync(cancellationToken);

		await transaction.CommitAsync(cancellationToken);
	}
}
