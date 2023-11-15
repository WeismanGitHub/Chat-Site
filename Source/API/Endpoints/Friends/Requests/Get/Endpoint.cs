namespace API.Endpoints.Friends.Requests.Get;

public sealed class Endpoint : Endpoint<Request, IReadOnlyList<FriendRequest>> {
    public override void Configure() {
        Get("/");
        Group<RequestGroup>();
        Version(1);
        
        Summary(settings => {
            settings.Summary = "Get friend requests.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
		var query = DB.PagedSearch<FriendRequest>()
			.PageSize(10)
			.PageNumber(req.Page ?? 0)
			.Sort(s => s.CreatedAt, MongoDB.Entities.Order.Ascending);

        if (req.FriendReqType == FriendRequestType.Incoming) {
			query.Match(fr => fr.RecipientID == req.AccountID);
        } else {
			query.Match(fr => fr.RequesterID == req.AccountID);
        }

        var res = (await query.ExecuteAsync()).Results;
		await SendAsync(res);
    }
}
