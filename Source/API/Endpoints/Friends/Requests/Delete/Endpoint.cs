namespace API.Endpoints.Friends.Requests.Delete;

public class Endpoint : Endpoint<Request> {
    public override void Configure() {
        Delete("/{RequestID}");
        Group<RequestGroup>();
        Version(1);

        Description(builder => builder.Accepts<Request>());

        Summary(settings => settings.Summary = "Delete a friend request you sent.");
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        var friendRequest = await DB.Find<FriendRequest>().MatchID(req.RequestID).ExecuteSingleAsync(cancellationToken);

        if (friendRequest == null) {
            ThrowError("Could not find friend request,", 404);
        } else if (friendRequest.RequesterID != req.AccountID) {
            ThrowError("You can only delete requests you've sent.", 403);
        } else if (friendRequest.Status != Status.Pending) {
            ThrowError("You can only delete pending requests.", 405);
        }

        await friendRequest.DeleteAsync();
    }
}
