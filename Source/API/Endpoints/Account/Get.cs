namespace API.Endpoints.Account.Get;

public sealed class Request {
	[From(Claim.AccountID, IsRequired = true)]
	public string AccountID { get; set; }
}

public sealed class Response {
	public required string ID { get; set; }
	public required string DisplayName { get; set; }
	public required string Email { get; set; }
	public int ChatRooms { get; set; }
	public DateTime CreatedAt { get; set; }
}

public sealed class Endpoint : Endpoint<Request, Response> {
	public override void Configure() {
		Get("/");
		Group<AccountGroup>();
		Version(1);
	}

	public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
		var user = await DB.Find<User>().OneAsync(req.AccountID, cancellationToken);

		if (user == null) {
			ThrowError("Could not find your account.", 404);
		}

		await SendAsync(new() {
			ID = req.AccountID,
			DisplayName = user.DisplayName,
			Email = user.Email,
			ChatRooms = user.ChatRoomIDs.Count,
			CreatedAt = user.CreatedAt,
		}, cancellation: cancellationToken);
	}
}
