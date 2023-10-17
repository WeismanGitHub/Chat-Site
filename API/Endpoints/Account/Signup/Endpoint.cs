namespace API.Endpoints.Account.Signup;

internal sealed class Endpoint : Endpoint<Request, Response, Mapper> {
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
        var account = Map.ToEntity(req);

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
    }
}
