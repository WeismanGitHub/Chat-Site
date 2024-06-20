namespace API.Endpoints.ChatRooms.SingleChatRoom.Leave;

public sealed class Request {
	[From(Claim.AccountID, IsRequired = true)]
	public string AccountID { get; set; }
	public required string ChatRoomID { get; set; }

}

public sealed class Endpoint : Endpoint<Request> {
	public override void Configure() {
		Post("/{ChatRoomID}/leave");
		Group<ChatRoomGroup>();
		Version(1);

		Description(builder => builder.Accepts<Request>());

		Summary(settings => {
			settings.Summary = "Leave a chat room.";
		});
	}

	public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
		var transaction = new Transaction();

		var chatUpdateRes = await transaction.UpdateAndGet<ChatRoom>()
			.MatchID(req.ChatRoomID)
			.Modify(c => c.Pull(c => c.MemberIDs, req.AccountID))
			.ExecuteAsync();

		var userUpdateRes = await transaction.Update<User>()
			.MatchID(req.AccountID)
			.Modify(user => user.Pull(u => u.ChatRoomIDs, req.ChatRoomID))
			.ExecuteAsync();

		if (chatUpdateRes.MemberIDs.Count == 0) {
			await transaction.DeleteAsync<ChatRoom>(req.ChatRoomID);
		}

		await transaction.CommitAsync();

		if (chatUpdateRes == null || userUpdateRes.ModifiedCount != 1) {
			ThrowError("Something went wrong.", 500);
		} else if (userUpdateRes.MatchedCount == 0) {
			ThrowError("Could not find your account.", 404);
		}
	}
}
