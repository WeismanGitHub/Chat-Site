namespace API.Endpoints.Friends.Requests.Decline;

public class Endpoint : Endpoint<Request> {
	public override void Configure() {
		Post("/{RequestID}/decline");
		Group<RequestGroup>();
		Version(1);

		Description(builder => builder.Accepts<Request>());

		Summary(settings => {
			settings.Summary = "Decline a friend request.";
		});
	}

	public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
		var friendRequest = await DB.Find<FriendRequest>().MatchID(req.RequestID).ExecuteSingleAsync();

		if (friendRequest == null) {
			ThrowError("FriendRequest does not exist.", 404);
		} else if (friendRequest.RecipientID != req.AccountID) {
			ThrowError("You cannot decline this FriendRequest.", 403);
		} else if (friendRequest.Status != Status.Pending) {
			ThrowError("You can only decline pending requests.", 400);
		}

		friendRequest.Status = Status.Declined;
		await friendRequest.SaveAsync();
	}
}
