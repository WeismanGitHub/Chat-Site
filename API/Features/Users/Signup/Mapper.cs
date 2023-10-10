namespace API.Features.Users.Signup;

internal sealed class Mapper : Mapper<SignupReq, SignupRes, User> {
    public override User ToEntity(SignupReq req) => new() {
        Email = req.Email.ToLower(),
        DisplayName = req.DisplayName,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password)
    };
}
