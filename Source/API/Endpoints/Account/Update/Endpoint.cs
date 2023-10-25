namespace API.Endpoints.Account.Update;

sealed class Endpoint : Endpoint<Request> {
    public override void Configure() {
        Patch("/");
        Group<AccountGroup>();
        Version(1);

        Summary(settings => {
            settings.Summary = "Update logged in account.";
            settings.ExampleRequest = new Request {
                DisplayName = "New Name",
            };
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        if (
            req.DisplayName == null && 
            req.Email == null && 
            req.Password == null
        ) {
            ThrowError("Must modify something.", 400);
        }

        if (req.Email != null) {
            User? account = await DB.Find<User>().Match(u => u.ID == req.AccountID).ExecuteSingleAsync();

            if (account == null) {
                ThrowError("Could not find your account.", 404);
            }
        }

        await Data.Update(req);
    }
}
