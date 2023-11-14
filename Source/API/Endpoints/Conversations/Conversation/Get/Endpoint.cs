namespace API.Endpoints.Conversations.SingleConvo.Get;

public sealed class Endpoint : Endpoint<Request, Response> {
    public override void Configure() {
		Get("/{ConversationID}");
		Group<ConversationGroup>();
        Version(1);
        
        Summary(settings => {
            settings.Summary = "Get logged in account's conversations.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        var conversation = await DB
        .Find<Conversation>()
			.MatchID(req.ConversationID)
            .Match(c => c.MemberIDs.Contains(req.AccountID))
            .Project(c => new() {
				ID = c.ID,
				Name = c.Name,
				MemberIDs = c.MemberIDs,
				CreatedAt = c.CreatedAt
            })
            .ExecuteSingleAsync();

		if (conversation == null) {
			ThrowError("Could not find conversation.", 404);
		}

		var members = await DB.Find<User>()
			.Match(u => conversation.MemberIDs.Contains(u.ID))
			.Project(u => new() {
				ID = u.ID,
				DisplayName = u.DisplayName,
			})
			.ExecuteAsync();

		await SendAsync(new Response() {
			ID = conversation.ID,
			Name = conversation.Name,
			CreatedAt = conversation.CreatedAt,
			Members = members
		});
    }
}
