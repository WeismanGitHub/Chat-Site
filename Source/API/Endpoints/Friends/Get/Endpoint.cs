 namespace API.Endpoints.Friends.Get;

public sealed class Endpoint : Endpoint<Request, List<FriendDTO>> {
    public override void Configure() {
        Get("/");
        Group<FriendGroup>();
        Version(1);
        
        Summary(settings => {
            settings.Summary = "Get logged in account's friends.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        var account = await DB.Find<User>()
            .Project(u => new() { FriendIDs = u.FriendIDs })
            .OneAsync(req.AccountID, cancellationToken);

        if (account == null) {
            ThrowError("Could not find your account.", 404);
        }

        if (account.FriendIDs.Count == 0) {
            await SendAsync(null);
        }

        var friends = await DB
			.Find<User, FriendDTO>()
            .Match(u => account.FriendIDs.Contains(u.ID))
            .Project(u => new FriendDTO() {
                ID = u.ID,
                DisplayName = u.DisplayName,
                CreatedAt = u.CreatedAt
            })
            .ExecuteAsync(cancellationToken);

		await SendAsync(friends);
    }
}
