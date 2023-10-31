namespace API.Endpoints.Account.Update;

public sealed class Endpoint : Endpoint<Request> {
    public override void Configure() {
        Patch("/");
        Group<AccountGroup>();
        Version(1);

        Summary(settings => {
            settings.Summary = "Update logged in account.";
            settings.ExampleRequest = new Request {
				NewData = new() {
					DisplayName = "New Name",
				},
				CurrentPassword = "Password123"
			};
        });
    }

    public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
        var update = DB.Update<User>().MatchID(req.AccountID);
		var newData = req.NewData;

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
