using FastEndpoints.Security;

namespace API.Endpoints.Account.Signin;

public static class Data {
	internal static Task<User?> GetAccount(string email) {
		return DB
			.Find<User>()
			.Match(u => u.Email == email)
			.Project(u => new() {
				PasswordHash = u.PasswordHash,
				DisplayName = u.DisplayName,
				ID = u.ID,
			})
			.ExecuteSingleAsync();
	}
}

public sealed class Request {
	public required string Email { get; set; }
	public required string Password { get; set; }
}

internal sealed class Validator : Validator<Request> {
	public Validator() {
		RuleFor(account => account.Email)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress().WithMessage("The format of your email address is invalid.");

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
			settings.Summary = "Sign into an account.";
			settings.ExampleRequest = new Request {
				Email = "person1@email.com",
				Password = "Password123",
			};
		});
	}

	public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
		var account = await Data.GetAccount(req.Email);

		if (account == null) {
			ThrowError("Could not find an account with that email.", 400);
		} else if (!BCrypt.Net.BCrypt.Verify(req.Password, account.PasswordHash)) {
			ThrowError("Invalid Credentials", 400);
		}

		await CookieAuth.SignInAsync(u => u["AccountID"] = account.ID!);
	}
}
