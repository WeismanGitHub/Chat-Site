namespace API.Endpoints.Account.Get;

public sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
}

public sealed class Response {
	public string ID { get; set; }
	public string DisplayName { get; set; }
	public string Email { get; set; }
	public int ChatRooms { get; set; }
	public DateTime CreatedAt { get; set; }
}
