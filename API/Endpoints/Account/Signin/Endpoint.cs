using Microsoft.Extensions.Options;
using FastEndpoints.Security;
using API.Auth;

namespace API.Endpoints.Account.Signin;

internal sealed class Endpoint : Endpoint<Request, Response> {
    public IOptions<Settings> Settings { get; set; } = null!;

    public override void Configure() {
        Post("/Signin");
        Group<AccountGroup>();
        AllowAnonymous();
        Version(1);
        
        Summary(settings => {
            settings.Summary = "Sign into an account.";
            settings.ExampleRequest = new Request {
                Email = "person1@email.com",
                Password = "Password123",
            };
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        var account = await Data.GetAccount(req.Email);

        if (account == null) {
            ThrowError("Could not find account with that email.");
        } else if (!BCrypt.Net.BCrypt.Verify(req.Password, account.PasswordHash)) {
            ThrowError("Invalid Credentials");
        }

        var expiryDate = DateTime.UtcNow.AddMinutes(Settings.Value.Auth.TokenValidityMinutes);

        Response.Token.Expiry = expiryDate.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:ss");
        Response.Token.Value = JWTBearer.CreateToken(
            signingKey: Settings.Value.Auth.SigningKey,
            expireAt: expiryDate,
            privileges: u => { u[Claim.AccountID] = account.ID!; });
    }
}
