namespace API.Endpoints.Conversations.Get;

public sealed class Endpoint : Endpoint<Request, List<Conversation>> {
    public override void Configure() {
        Get("/");
        Group<ConversationGroup>();
        Version(1);
        
        Summary(settings => {
            settings.Summary = "Get logged in account's conversations.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        var account = await DB.Find<User>()
            .Project(u => new() { ConversationIDs = u.ConversationIDs })
            .OneAsync(req.AccountID);

        if (account == null) {
            ThrowError("Could not find your account.", 404);
        }

        if (account.ConversationIDs.Count == 0) {
            await SendAsync(new List<Conversation>());
        }

        var conversations = await DB
        .Find<Conversation>()
            .Match(c => account.ConversationIDs.Contains(c.ID))
            .Project(u => new() {
				ID = u.ID,
				Name = u.Name,
				CreatedAt = u.CreatedAt
            })
            .ExecuteAsync();

		await SendAsync(conversations);
    }
}
