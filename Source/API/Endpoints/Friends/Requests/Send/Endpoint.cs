using Microsoft.Extensions.Options;

using System.Security.Cryptography.Pkcs;

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
        var recipient = DB.Find<User>().OneAsync(req.AccountID);

        if (req.RecipientID == req.AccountID) {
            ThrowError("You cannot befriend yourself.", 400);
        } else if (recipient == null) {
            ThrowError("No users with that ID.", 400);
        }

        var friendReq = new FriendRequest() {
            Message = req.Message,
            RecipientId = req.RecipientID,
            RequesterId = req.AccountID,
        };

        await DB.InsertAsync(friendReq);
    }
}
