using FastEndpoints.Security;

namespace API.Endpoints.Account.Signup;

public sealed class Request {
	public required string Name { get; set; }
	public required string Password { get; set; }
}

internal sealed class Validator : Validator<Request> {
	public Validator() {
		RuleFor(account => account.Name)
			.NotEmpty().WithMessage("Name is required.")
			.MinimumLength(1).WithMessage("Name cannot be empty.")
			.MaximumLength(User.MaxNameLength).WithMessage($"Name cannot be longer than {User.MaxNameLength} characters.");

		RuleFor(account => account.Password)
			.NotEmpty().WithMessage("Password is required.")
			.Must(password => password.IsAValidPassword()).WithMessage("Password is invalid.");
	}
}

public static class Data {
	internal static Task<bool> NameIsTaken(string name) {
		return DB
			.Find<User>()
			.Match(u => u.Name == name)
			.ExecuteAnyAsync();
	}
}

public sealed class Endpoint : Endpoint<Request> {
	public override void Configure() {
		Post("/Signup");
		Group<AccountGroup>();
		AllowAnonymous();
		Version(1);

		Summary(settings => {
			settings.ExampleRequest = new Request {
				Name = "Person 1",
				Password = "Password123",
			};
		});
	}

	public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
		User account = new() {
			Name = req.Name,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password)
		};

		ThrowIfAnyErrors();

		try {
			await account.SaveAsync(cancellation: cancellationToken);
		} catch (Exception) {
			var NameIsTaken = await Data.NameIsTaken(account.Name);

			if (NameIsTaken) {
				ThrowError(r => r.Name, "That name is unavailable.", 409);
			}

			throw;
		}

		await CookieAuth.SignInAsync(u => u["AccountID"] = account.ID!);
	}
}
