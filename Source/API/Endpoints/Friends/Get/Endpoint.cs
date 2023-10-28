using API.Database.Entities;

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
        var account = await DB.Find<User>()
            .Project(u => new() { FriendIDs = u.FriendIDs })
            .OneAsync(req.AccountID);

        if (account == null) {
            ThrowError("Could not find your account.", 404);
        }

        if (account.FriendIDs.Count == 0) {
            return new List<User>();
        }

        return await DB
        .Find<User>()
            .Match(u => account.FriendIDs.Contains(u.ID))
            .Project(u => new() {
                DisplayName = u.DisplayName,
                ID = u.ID,
                CreatedAt = u.CreatedAt
            })
            .ExecuteAsync();
    }
}
