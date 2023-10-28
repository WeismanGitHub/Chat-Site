namespace API.Endpoints.Friends.Requests.Send;

public sealed class Endpoint : Endpoint<Request> {
    public override void Configure() {
        Post("/");
        Group<RequestGroup>();
        Version(1);
        
        Summary(settings => {
            settings.Summary = "Send a friend request to a user.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        var recipient = await DB.Find<User>()
            .MatchID(req.RecipientID)
            .ExecuteSingleAsync();

        if (recipient == null) {
            ThrowError("Could not find user.", 404);
        } else if (recipient.FriendIDs.Contains(req.AccountID)) {
            ThrowError("You're already friends with this user.", 400);
        }

        var friendReq = new FriendRequest() {
            Message = req.Message,
            RecipientID = req.RecipientID,
            RequesterID = req.AccountID,
        };

        await DB.InsertAsync(friendReq);
    }
}
