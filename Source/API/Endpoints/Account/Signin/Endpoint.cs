using FastEndpoints.Security;

namespace API.Endpoints.Account.Signin;

public sealed class Endpoint : Endpoint<Request> {
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
            ThrowError("Could not find an account with that email.", 400);
        } else if (!BCrypt.Net.BCrypt.Verify(req.Password, account.PasswordHash)) {
            ThrowError("Invalid Credentials", 400);
        }

        await CookieAuth.SignInAsync(u => u["AccountID"] = account.ID!);
    }
}
