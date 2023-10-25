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
        var user = await DB.Find<User>().OneAsync(req.AccountID);

        if (user == null) {
            ThrowError("Could not find user.", 404);
        }

        await Data.DeleteUser(user);
        await CookieAuth.SignOutAsync();
    }
}
