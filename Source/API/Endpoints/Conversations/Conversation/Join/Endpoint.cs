namespace API.Endpoints.Conversations.SingleConvo.Join;

public sealed class Endpoint : Endpoint<Request> {
    public override void Configure() {
		Post("/{ConversationID}/join");
		Group<ConversationGroup>();
        Version(1);

		Description(builder => builder.Accepts<Request>());

        Summary(settings => {
            settings.Summary = "Join a conversation.";
        });
    }

	public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
		var conversation = await DB.Find<Conversation>().MatchID(req.ConversationID).ExecuteSingleAsync();

		if (conversation == null) {
			ThrowError("Could not find conversation", 404);
		} else if (conversation.MemberIDs.Count >= 100) {
			ThrowError("Conversation is at capacity.", 400);
		} else if (conversation.MemberIDs.Contains(req.AccountID)) {
			ThrowError("You are already a member of this conversation.", 400);
		}

		var transaction = new Transaction();

		conversation.MemberIDs.Add(req.AccountID);
		await transaction.SaveAsync(conversation);

		await transaction.Update<User>()
			.MatchID(req.AccountID)
			.Modify("{ $push: { 'ConversationIDs': { " + req.ConversationID + " } } }")
			.ExecuteAsync();

		await transaction.CommitAsync();
    }
}
