using FastEndpoints.Security;

namespace API.Endpoints.Account.Signup;

public sealed class Request {
	public required string DisplayName { get; set; }
	public required string Email { get; set; }
	public required string Password { get; set; }
}

internal sealed class Validator : Validator<Request> {
	public Validator() {
		RuleFor(account => account.DisplayName)
			.NotEmpty().WithMessage("DisplayName is required.")
			.MinimumLength(1).WithMessage("DisplayName cannot be empty.")
			.MaximumLength(User.MaxNameLength).WithMessage($"DisplayName cannot be longer than {User.MaxNameLength} characters.");

		RuleFor(account => account.Email)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress().WithMessage("The format of your email address is invalid.");

		RuleFor(account => account.Password)
			.NotEmpty().WithMessage("Password is required.")
			.Must(password => password.IsAValidPassword()).WithMessage("Password is invalid.");
	}
}

public static class Data {
	internal static Task<bool> EmailAddressIsTaken(string email) {
		return DB
			.Find<User>()
			.Match(u => u.Email == email)
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
				DisplayName = "Person 1",
				Email = "person1@email.com",
				Password = "Password123",
			};
		});
	}

	public override async Task HandleAsync(Request req, CancellationToken cancellationToken) {
		User account = new() {
			Email = req.Email.ToLower(),
			DisplayName = req.DisplayName,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password)
		};

		ThrowIfAnyErrors();

		try {
			await account.SaveAsync(cancellation: cancellationToken);
		} catch (Exception) {
			var emailIsTaken = await Data.EmailAddressIsTaken(account.Email);

			if (emailIsTaken) {
				ThrowError(r => r.Email, "That email is unavailable.", 409);
			}

			throw;
		}

		await CookieAuth.SignInAsync(u => u["AccountID"] = account.ID!);
	}
}
