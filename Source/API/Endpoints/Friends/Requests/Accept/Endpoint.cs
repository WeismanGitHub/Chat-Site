namespace API.Endpoints.Friends.Requests.Accept;

public class Endpoint : Endpoint<Request> {
    public override void Configure() {
        Post("/{RequestID}/accept");
        Group<RequestGroup>();
        Version(1);

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

        Console.WriteLine(friendRequest.Message);
    }
}
