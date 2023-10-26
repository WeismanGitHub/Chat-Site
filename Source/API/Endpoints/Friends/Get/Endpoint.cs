namespace API.Endpoints.Friends.Get;

public sealed class Endpoint : Endpoint<Request> {
    public override void Configure() {
        Get("/");
        Group<FriendGroup>();
        Version(1);
        
        Summary(settings => {
            settings.Summary = "Get logged in account's friends.";
        });
    }

    public override async Task<List<User>> HandleAsync(Request req, CancellationToken cancellationToken) {
        var account = await DB.Find<User>().OneAsync(req.AccountID);

        if (account == null) {
            ThrowError("Could not find your account.", 404);
        }

        return await Data.GetFriends(account);
    }
}
