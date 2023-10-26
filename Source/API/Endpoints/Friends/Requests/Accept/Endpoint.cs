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
        Console.WriteLine(friendRequest.Message);
    }
}
