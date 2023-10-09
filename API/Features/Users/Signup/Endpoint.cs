namespace API.Features.Users.Signup;

internal sealed class Endpoint : Endpoint<Request, Response, Mapper> {
    public override void Configure() {
        Post("/Signup");
        Group<UsersGroup>();
        AllowAnonymous();
        
        Summary(settings => {
            settings.Summary = "Create a user.";
            settings.ExampleRequest = new Request {
                DisplayName = "Person 1",
                Email = "person1@email.com",
                Password = "Password123",
            };
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        await SendAsync(new Response() {
            Message = "created!"
        });
    }
}
