namespace API.Endpoints.Account.Get;

sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
}

sealed class Response {
	public string ID { get; set; }
	public string DisplayName { get; set; }
	public string Email { get; set; }
	public int TotalFriends { get; set; }
	public int TotalConversations { get; set; }
	public DateTime CreatedAt { get; set; }
}
