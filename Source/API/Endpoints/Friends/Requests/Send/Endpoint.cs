using Microsoft.Extensions.Options;

namespace API.Endpoints.Friends.Requests.Send;

public sealed class Endpoint : Endpoint<Request> {
    public IOptions<Settings> Settings { get; set; } = null!;

    public override void Configure() {
        Post("/");
        Group<RequestGroup>();
        Version(1);
        
        Summary(settings => {
            settings.Summary = "Send a friend request to a user.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        var friendReq = new FriendRequest() {
            Message = req.Message,
            RecipientID = req.RecipientID,
            RequesterID = req.AccountID,
        };

        await DB.InsertAsync(friendReq);
    }
}
