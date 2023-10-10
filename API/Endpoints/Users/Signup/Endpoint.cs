namespace API.Endpoints.Users.Signup;

internal sealed class Endpoint : Endpoint<SignupReq, SignupRes, Mapper> {
    public override void Configure() {
        Post("/Signup");
        Group<UsersGroup>();
        AllowAnonymous();
        
        Summary(settings => {
            settings.Summary = "Create a user.";
            settings.ExampleRequest = new SignupReq {
                DisplayName = "Person 1",
                Email = "person1@email.com",
                Password = "Password123",
            };
        });
    }

    public override async Task HandleAsync(SignupReq req, CancellationToken cancellationToken) {
        var user = Map.ToEntity(req);

        ThrowIfAnyErrors();

        try {
            await user.SaveAsync();
        } catch (Exception) {
            var emailIsTaken = await Data.EmailAddressIsTaken(user.Email);

            if (emailIsTaken) {
                ThrowError(r => r.Email, "That Email is unavailable.");
            }

            throw;
        }

        await SendAsync(new SignupRes() {
            Message = "Signed up!"
        });
    }
}
