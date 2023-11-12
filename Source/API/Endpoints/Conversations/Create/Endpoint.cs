namespace API.Endpoints.Conversations.Create;

public sealed class Endpoint : Endpoint<Request, List<User>> {
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

		//var convo = await DB.InsertAsync<Conversa>
  //      await DB.Update<User>()
  //          .M(u => account.FriendIDs.Contains(u.ID))
  //          .Project(u => new() {
  //              DisplayName = u.DisplayName,
  //              ID = u.ID,
  //              CreatedAt = u.CreatedAt
  //          })
  //          .ExecuteAsync();
    }
}
