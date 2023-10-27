namespace API.Endpoints.Friends.Requests.Accept;

public class Endpoint : Endpoint<Request> {
    public override void Configure() {
        Post("/{RequestID}/accept");
        Group<RequestGroup>();
        Version(1);

        Description(builder => builder.Accepts<Request>());

        Summary(settings => {
            settings.Summary = "Accept a friend request.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        var friendRequest = await DB.Find<FriendRequest>().MatchID(req.RequestID).ExecuteSingleAsync();

        if (friendRequest == null) {
            ThrowError("FriendRequest does not exist.", 404);
        } else if (friendRequest.RecipientID != req.AccountID) {
            ThrowError("You cannot accept this FriendRequest.", 403);
        }

        // These should be whatever the equivalent of Promise.all is in C#.
        var recipient = await DB.Find<User>().MatchID(friendRequest.RecipientID).ExecuteSingleAsync();
        var requester = await DB.Find<User>().MatchID(friendRequest.RequesterID).ExecuteSingleAsync();

        if (requester == null) {
            ThrowError("Could not find requester.", 404);
        } else if (recipient == null) {
            ThrowError("Could not find your account.", 401);
        }

        var transaction = DB.Transaction();
        await requester.Friends.AddAsync(recipient, transaction.Session);
        await recipient.Friends.AddAsync(requester, transaction.Session);
        await transaction.CommitAsync();
    }
}
