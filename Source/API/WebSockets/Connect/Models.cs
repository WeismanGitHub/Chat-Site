namespace API.WebSockets.Connect;

public sealed class ConnectionRequest {
	[From(Claim.AccountID, IsRequired = true)]
	public string AccountID { get; set; }
}
