namespace API.Endpoints.Friends.Get;

public sealed class Endpoint : Endpoint<Request, List<FriendResponse>> {
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
            .OneAsync(req.AccountID);

        if (account == null) {
            ThrowError("Could not find your account.", 404);
        }

        if (account.FriendIDs.Count == 0) {
            await SendAsync(new());
        }

        var friends = await DB
        .Find<User, FriendResponse>()
            .Match(u => account.FriendIDs.Contains(u.ID))
            .Project(u => new FriendResponse() {
                ID = u.ID,
                DisplayName = u.DisplayName,
                CreatedAt = u.CreatedAt
            })
            .ExecuteAsync();

		await SendAsync(friends);
    }
}
