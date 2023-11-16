namespace API.Endpoints.Friends.Remove;

public sealed class Endpoint : Endpoint<Request> {
    public override void Configure() {
        Post("/remove");
        Group<FriendGroup>();
        Version(1);
        
        Summary(settings => {
            settings.Summary = "Remove a friend.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        var account = await DB.Find<User>().OneAsync(req.AccountID);
        var friend = await DB.Find<User>().OneAsync(req.AccountID);

		if (account == null) {
			ThrowError("Could not find your account.", 404);
		} else if (friend == null) {
			ThrowError("Could not find friend.", 404);
		}

		bool wereFriends = account.FriendIDs.Remove(friend.ID) && friend.FriendIDs.Remove(account.ID);

		if (!wereFriends) {
			ThrowError("You were not friends.", 400);
		}

		await account.SaveAsync();
		await friend.SaveAsync();
    }
}
