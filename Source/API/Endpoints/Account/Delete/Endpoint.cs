using FastEndpoints.Security;

namespace API.Endpoints.Account.Delete;

sealed class Endpoint : Endpoint<Request> {
    public override void Configure() {
        Delete("/");
        Group<AccountGroup>();
        Version(1);

        Summary(settings => {
            settings.Summary = "Delete logged in account.";
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        await Data.DeleteUser(req.AccountID);
        await CookieAuth.SignOutAsync();
    }
}
