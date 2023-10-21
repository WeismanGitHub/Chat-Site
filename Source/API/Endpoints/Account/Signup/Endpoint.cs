using FastEndpoints.Security;

namespace API.Endpoints.Account.Signup;

public sealed class Endpoint : Endpoint<Request> {
    public override void Configure() {
        Post("/Signup");
        Group<AccountGroup>();
        AllowAnonymous();
        Version(1);
        
        Summary(settings => {
            settings.Summary = "Create an account.";
            settings.ExampleRequest = new Request {
                DisplayName = "Person 1",
                Email = "person1@email.com",
                Password = "Password123",
            };
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        User account = new() {
            Email = req.Email.ToLower(),
            DisplayName = req.DisplayName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password)
        };

        ThrowIfAnyErrors();

        try {
            await account.SaveAsync();
        } catch (Exception) {
            var emailIsTaken = await Data.EmailAddressIsTaken(account.Email);

            if (emailIsTaken) {
                ThrowError(r => r.Email, "That Email is unavailable.");
            }

            throw;
        }

        await CookieAuth.SignInAsync(u => u["AccountID"] = account.ID!);
    }
}
