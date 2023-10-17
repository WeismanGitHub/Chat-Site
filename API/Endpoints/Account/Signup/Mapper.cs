namespace API.Endpoints.Account.Signup;

internal sealed class Mapper : Mapper<Request, Response, User> {
    public override User ToEntity(Request req) => new() {
        Email = req.Email.ToLower(),
        DisplayName = req.DisplayName,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password)
    };
}
