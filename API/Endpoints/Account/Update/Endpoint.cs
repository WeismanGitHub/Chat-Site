namespace API.Endpoints.Account.Update;

internal sealed class Endpoint : Endpoint<Request, Response, Mapper> {
    public override void Configure() {
        Patch("/Update");
        Group<AccountGroup>();
        Version(1);

        Summary(settings => {
            settings.Summary = "Update an account.";
            settings.ExampleRequest = new Request {
                DisplayName = "New Name",
            };
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        var account = Map.ToEntity(req);

        await Data.Update(account);
    }
}
