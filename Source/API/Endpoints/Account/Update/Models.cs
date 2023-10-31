using System.Net;

namespace API.Endpoints.Account.Update;

public class NewData {
	public string? DisplayName { get; set; }
	public string? Email { get; set; }
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
				User? account = await DB.Find<User>().MatchID(req.AccountID).ExecuteSingleAsync();
				return account == null ? false : BCrypt.Net.BCrypt.Verify(req.CurrentPassword, account.PasswordHash);
			})
			.WithErrorCode(nameof(HttpStatusCode.Unauthorized))
			.WithMessage("Invalid password.");

		RuleFor(req => req.NewData)
            .Must(req => req.DisplayName != null || req.Email != null || req.Password != null)
            .WithMessage("Must modify something");

		When(req => req.NewData.DisplayName != null, () => {
			RuleFor(account => account.NewData.DisplayName)
			.NotEmpty()
			.MinimumLength(1).WithMessage("DisplayName cannot be empty.")
			.MaximumLength(User.MaxNameLength).WithMessage($"DisplayName cannot be longer than {User.MaxNameLength} characters.");
		});

		When(req => req.NewData.Email != null, () => {
			RuleFor(account => account.NewData.Email)
				.NotEmpty()
				.EmailAddress().WithMessage("The format of your email address is invalid.")
				.MustAsync(async (email, _) => await DB.Find<User>().Match(u => u.Email == email).ExecuteAnyAsync())
				.WithErrorCode(HttpStatusCode.Conflict.ToString())
				.WithMessage("Email is taken.");
		});

		When(req => req.NewData.Password != null, () => {
			RuleFor(account => account.NewData.Password)
				.NotEmpty()
				.Must(password => {
					if (password == null) return true;
					return password.IsAValidPassword();
				}).WithMessage("Password is invalid.");
		});
	}
}
