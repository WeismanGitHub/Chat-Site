using FastEndpoints.Security;

namespace API.Endpoints.Account.Delete;

internal sealed class Request {
	[From(Claim.AccountID, IsRequired = true)]
	public string AccountID { get; set; }
}

internal sealed class Endpoint : Endpoint<Request> {
	public override void Configure() {
		Delete("/");
		Group<AccountGroup>();
		Version(1);
	}

	public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
		var account = await DB.Find<User>().OneAsync(req.AccountID, cancellationToken);

		if (account == null) {
			ThrowError("Could not find your account.", 404);
		}

		var transaction = new Transaction();

		var chat = await transaction.UpdateAndGet<ChatRoom>()
			.Match(c => account.ChatRoomIDs.Contains(c.ID))
			.Modify(c => c.Pull<string>(c => c.MemberIDs, account.ID))
			.ExecuteAsync(cancellationToken);

		if (chat.MemberIDs.Count == 0) {
			await transaction.DeleteAsync<ChatRoom>(chat.ID);
		}

		await transaction.DeleteAsync<User>(account.ID);

		await transaction.CommitAsync(cancellationToken);
		await CookieAuth.SignOutAsync();
	}
}
