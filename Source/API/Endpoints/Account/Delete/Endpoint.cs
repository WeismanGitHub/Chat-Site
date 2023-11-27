using FastEndpoints.Security;

namespace API.Endpoints.Account.Delete;

sealed class Endpoint : Endpoint<Request> {
    public override void Configure() {
        Delete("/");
        Group<AccountGroup>();
        Version(1);

        Summary(settings => settings.Summary = "Delete logged in account.");
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        var account = await DB.Find<User>().OneAsync(req.AccountID);

        if (account == null) {
            ThrowError("Could not find your account.", 404);
        }

		var transaction = new Transaction();

		var convo = await transaction.UpdateAndGet<Conversation>()
			.Match(convo => account.ConversationIDs.Contains(convo.ID))
			.Modify(convo => convo.Pull(c => c.MemberIDs, account.ID))
			.ExecuteAsync();

		if (convo.MemberIDs.Count == 0) {
			await transaction.DeleteAsync<Conversation>(convo.ID);
		}

		await transaction.Update<User>()
			.Match(user => account.FriendIDs.Contains(user.ID))
			.Modify(user => user.Pull(u => u.FriendIDs, account.ID))
			.ExecuteAsync();

		await transaction.DeleteAsync<User>(account.ID);
		await transaction.DeleteAsync<FriendRequest>(fr => fr.RequesterID == account.ID);

		await transaction.CommitAsync();
		await CookieAuth.SignOutAsync();
    }
}
