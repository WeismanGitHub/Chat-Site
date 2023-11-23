namespace API.Endpoints.Friends.Remove;

public sealed class Endpoint : Endpoint<Request> {
    public override void Configure() {
        Post("/{FriendID}/remove");
        Group<FriendGroup>();
        Version(1);
        
        Summary(settings => {
            settings.Summary = "Remove a friend.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        var account = await DB.Find<User>().OneAsync(req.AccountID, cancellationToken);

		if (account == null) {
			ThrowError("Could not find your account.", 404);
		} else if (!account.FriendIDs.Contains(req.FriendID)) {
			ThrowError("You are not friends.", 400);
		}

        var friend = await DB.Find<User>().OneAsync(req.FriendID, cancellationToken);

		if (friend == null) {
			ThrowError("Cannot find friend.", 404);
		}

		account.FriendIDs.Remove(friend.ID);
		friend.FriendIDs.Remove(account.ID);

		await account.SaveAsync(null, cancellationToken);
		await friend.SaveAsync(null, cancellationToken);
    }
}
