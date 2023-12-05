namespace API.Endpoints.Friends.Requests.Get;

public sealed class Endpoint : Endpoint<Request, Response> {
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
			.PageNumber(req.Page ?? 1)
			.Sort(s => s.CreatedAt, MongoDB.Entities.Order.Ascending);

        if (req.Type == FriendRequestType.Incoming) {
			query.Match(fr => fr.RecipientID == req.AccountID);
        } else {
			query.Match(fr => fr.RequesterID == req.AccountID);
        }

        var (results, totalCount, _) = await query.ExecuteAsync(cancellationToken);

		if (results == null) {
			ThrowError("Could not find results.", 404);
		}

		await SendAsync(new Response() {
			FriendRequests = results,
			TotalCount = totalCount,
		});
    }
}
