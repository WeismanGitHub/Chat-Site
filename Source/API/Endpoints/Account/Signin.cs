using FastEndpoints.Security;

namespace API.Endpoints.Account.Signin;

public static class Data {
	internal static Task<User?> GetAccount(string name) {
		return DB
			.Find<User>()
			.Match(u => u.Name == name)
			.ExecuteSingleAsync();
	}
}

public sealed class Request {
	public required string Name { get; set; }
	public required string Password { get; set; }
}

internal sealed class Validator : Validator<Request> {
	public Validator() {
		RuleFor(account => account.Name)
			.NotEmpty()
			.WithMessage("Name is required.");

		RuleFor(account => account.Password)
			.NotEmpty().WithMessage("Password is required.")
			.MinimumLength(User.MinPasswordLength).WithMessage($"Password must be at least {User.MinPasswordLength} characters.")
			.MaximumLength(User.MaxPasswordLength).WithMessage($"Password cannot be longer than {User.MaxPasswordLength} characters.");
	}
}

public sealed class Endpoint : Endpoint<Request> {
	public override void Configure() {
		Post("/Signin");
		Group<AccountGroup>();
		AllowAnonymous();
		Version(1);

		Summary(settings => {
			settings.ExampleRequest = new Request {
				Name = "Username",
				Password = "Password123",
			};
		});
	}

	public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
		var account = await Data.GetAccount(req.Name);

		if (account == null) {
			ThrowError("Could not find an account with that email.", 400);
		} else if (!BCrypt.Net.BCrypt.Verify(req.Password, account.PasswordHash)) {
			ThrowError("Invalid Credentials", 400);
		}

		await CookieAuth.SignInAsync(u => u["AccountID"] = account.ID!);
	}
}
