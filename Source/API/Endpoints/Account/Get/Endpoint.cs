namespace API.Endpoints.Account.Get;

public sealed class Endpoint : Endpoint<Request, Response> {
    public override void Configure() {
        Get("/");
        Group<AccountGroup>();
        Version(1);

        Summary(settings => {
            settings.Summary = "Get logged in account information.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        var user = await DB.Find<User>().OneAsync(req.AccountID);

        if (user == null) {
            ThrowError("Could not find your account.", 404);
        }

		await SendAsync(new() {
			ID = req.AccountID,
			DisplayName = user.DisplayName,
			Email = user.Email,
			TotalConversations = user.ConversationIDs.Count(),
			TotalFriends = user.FriendIDs.Count(),
			CreatedAt = user.CreatedAt,
		});
    }
}
