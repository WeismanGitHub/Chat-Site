namespace API.Endpoints.Account.Delete;

sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
}
