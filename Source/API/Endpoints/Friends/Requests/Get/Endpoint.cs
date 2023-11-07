using System.Reflection.Metadata.Ecma335;

namespace API.Endpoints.Friends.Requests.Get;

public sealed class Endpoint : Endpoint<Request> {
    public override void Configure() {
        Post("/");
        Group<RequestGroup>();
        Version(1);
        
        Summary(settings => {
            settings.Summary = "Send a friend request to a user.";
        });
    }

    public override async Task<List<FriendRequest>> HandleAsync(Request req, CancellationToken cancellationToken) {
		var query = DB.PagedSearch<FriendRequest>().PageSize(10).PageNumber(1);

        if (req.FriendReqType == FriendRequestType.Incoming) {
			query.Match(fr => fr.RecipientID == req.AccountID);
        } else {
			query.Match(fr => fr.RequesterID == req.AccountID);
        }

        var res = await query.ExecuteAsync();
		return res.Results.ToList();
    }
}
