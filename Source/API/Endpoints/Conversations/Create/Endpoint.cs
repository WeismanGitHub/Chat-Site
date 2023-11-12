namespace API.Endpoints.Conversations.Create;

public sealed class Endpoint : Endpoint<Request, Response> {
    public override void Configure() {
        Post("/");
        Group<ConversationGroup>();
        Version(1);
        
        Summary(settings => {
            settings.Summary = "Create a conversation.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        var account = await DB.Find<User>()
            .Project(u => new() { FriendIDs = u.FriendIDs })
            .OneAsync(req.AccountID);

        if (account == null) {
            ThrowError("Could not find your account.", 404);
        } else if (account.ConversationIDs.Count() >= 100) {
			ThrowError("Cannot join more than 100 conversations.", 400);
		}

		var convoID = ObjectId.GenerateNewId().ToString();
		await DB.InsertAsync(new Conversation() {
			ID = convoID,
			Name = req.ConversationName,
			MemberIDs = new List<string>() { account.ID }
		});

		account.ConversationIDs.Add(convoID);
		await account.SaveAsync();
		
		await SendAsync(new() {
			ConversationID = convoID
		});
	}
}
