using System.Net;

namespace API.Endpoints.Account.Update;

public class NewData {
	public string? Name { get; set; }
	public string? Password { get; set; }
}

public sealed class Request {
	[From(Claim.AccountID, IsRequired = true)]
	public string AccountID { get; set; }
	public required NewData NewData { get; set; }
	public required string CurrentPassword { get; set; }
}

internal sealed class Validator : Validator<Request> {
	public Validator() {
		RuleFor(req => req)
			.MustAsync(async (req, _) => {
				var account = await DB.Find<User>().MatchID(req.AccountID).ExecuteSingleAsync();
				return account != null && BCrypt.Net.BCrypt.Verify(req.CurrentPassword, account.PasswordHash);
			})
			.WithErrorCode(nameof(HttpStatusCode.Unauthorized))
			.WithMessage("Invalid password.");

		When(req => req.NewData.Name != null, () => {
			RuleFor(account => account.NewData.Name)
			.NotEmpty()
			.MinimumLength(1).WithMessage("Name cannot be empty.")
			.MaximumLength(User.MaxNameLength).WithMessage($"Name cannot be longer than {User.MaxNameLength} characters.");
		});

		When(req => req.NewData.Name != null, () => {
			RuleFor(account => account.NewData.Name)
				.NotEmpty()
				.MustAsync(async (name, _) => !await DB.Find<User>().Match(u => u.Name == name).ExecuteAnyAsync(_))
				.WithErrorCode(nameof(HttpStatusCode.Conflict))
				.WithMessage("Name is taken.");
		});

		When(req => req.NewData.Password != null, () => {
			RuleFor(account => account.NewData.Password)
				.NotEmpty()
				.Must(password => password == null || password.IsAValidPassword()).WithMessage("Password is invalid.");
		});
	}
}

public sealed class Endpoint : Endpoint<Request> {
	public override void Configure() {
		Patch("/");
		Group<AccountGroup>();
		Version(1);

		Summary(settings => {
			settings.ExampleRequest = new Request {
				NewData = new() {
					Name = "New Name",
				},
				CurrentPassword = "Password123"
			};
		});
	}

	public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
		var update = DB.Update<User>().MatchID(req.AccountID);
		var newData = req.NewData;

		if (newData.Name != null) {
			update.Modify(u => u.Name, newData.Name);
		}

		if (newData.Password != null) {
			var passwordHash = BCrypt.Net.BCrypt.HashPassword(newData.Password);
			update.Modify(u => u.PasswordHash, passwordHash);
		}

		await update.ExecuteAsync(cancellationToken);
	}
}
