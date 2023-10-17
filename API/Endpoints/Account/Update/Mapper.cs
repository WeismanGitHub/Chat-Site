namespace API.Endpoints.Account.Update;

internal sealed class AccountData {
    public string ID { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}
internal sealed class Mapper : Mapper<Request, Response, AccountData> {
    public override AccountData ToEntity(Request req) => new() {
        Email = req.Email.ToLower(),
        DisplayName = req.DisplayName,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password)
    };
}
