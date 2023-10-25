namespace API.Endpoints.Friends.Get;

public sealed class Friend {
    public string ID { get; set; }
    public string DisplayName { get; set; }
    public DateTime CreatedAt { get; set; }
}

public sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
}
