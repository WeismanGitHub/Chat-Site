namespace API.Features.Users.Signup;

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

        var emailIsTaken = await Data.EmailAddressIsTaken(user.Email);

        if (emailIsTaken) {
            AddError(r => r.Email, "That Email is unavailable.");
        }

        ThrowIfAnyErrors();

        await Data.CreateNewUser(user);

        await SendAsync(new SignupRes() {
            Message = "Signed up!"
        });
    }
}
