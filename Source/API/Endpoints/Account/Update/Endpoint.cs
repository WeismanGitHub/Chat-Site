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

    public override async Task HandleAsync(Request newData, CancellationToken cancellationToken) {
        var update = DB.Update<User>().MatchID(newData.AccountID);

        if (newData.DisplayName != null) {
            update.Modify(u => u.DisplayName, newData.DisplayName);
        }

        if (newData.Email != null) {
            update.Modify(u => u.Email, newData.Email);
        }

        if (newData.Password != null) {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(newData.Password);
            update.Modify(u => u.PasswordHash, passwordHash);
        }

        await update.ExecuteAsync(cancellationToken);
    }
}
