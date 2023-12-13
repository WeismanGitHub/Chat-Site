namespace API.Endpoints.Conversations.SingleConvo.Leave;

public sealed class Endpoint : Endpoint<Request> {
	public override void Configure() {
		Post("/{ConversationID}/leave");
		Group<ConversationGroup>();
		Version(1);

		Description(builder => builder.Accepts<Request>());

		Summary(settings => {
			settings.Summary = "Leave a conversation.";
		});
	}

	public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
		var transaction = new Transaction();

		var convoUpdateRes = await transaction.UpdateAndGet<Conversation>()
			.MatchID(req.ConversationID)
			.Modify(convo => convo.Pull(c => c.MemberIDs, req.AccountID))
			.ExecuteAsync();

		var userUpdateRes = await transaction.Update<User>()
			.MatchID(req.AccountID)
			.Modify(user => user.Pull(u => u.ConversationIDs, req.ConversationID))
			.ExecuteAsync();

		if (convoUpdateRes.MemberIDs.Count == 0) {
			await transaction.DeleteAsync<Conversation>(req.ConversationID);
		}

		await transaction.CommitAsync();

		if (convoUpdateRes == null || userUpdateRes.ModifiedCount != 1) {
			ThrowError("Something went wrong.", 500);
		} else if (userUpdateRes.MatchedCount == 0) {
			ThrowError("Could not find your account.", 404);
		}
	}
}
