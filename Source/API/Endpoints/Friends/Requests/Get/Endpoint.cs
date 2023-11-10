namespace API.Endpoints.Friends.Requests.Get;

public sealed class Endpoint : Endpoint<Request> {
    public override void Configure() {
        Get("/");
        Group<RequestGroup>();
        Version(1);
        
        Summary(settings => {
            settings.Summary = "Get friend requests.";
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
