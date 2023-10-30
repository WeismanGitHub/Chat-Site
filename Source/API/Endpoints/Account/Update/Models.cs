using System.Net;

namespace API.Endpoints.Account.Update;

sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string? AccountID { get; set; }
    public string? DisplayName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}

internal sealed class Validator : Validator<Request> {
    public Validator() {
        RuleFor(req => req)
            .Must(req => req.DisplayName != null || req.Email != null || req.Password != null)
            .WithMessage("Must modify something");

        RuleFor(account => account.DisplayName)
            .MinimumLength(1).WithMessage("DisplayName cannot be empty.")
            .MaximumLength(User.MaxNameLength).WithMessage($"DisplayName cannot be longer than {User.MaxNameLength} characters.");

        RuleFor(account => account.Email)
            .EmailAddress().WithMessage("The format of your email address is invalid.")
            .MustAsync(async (email, _) => await DB.Find<User>().Match(u => u.Email == email).ExecuteAnyAsync())
            .WithErrorCode(HttpStatusCode.Conflict.ToString())
            .WithMessage("Email is taken.");

        RuleFor(account => account.Password)
            .Must(password => {
                if (password == null) return true;
                return password.IsAValidPassword();
            }).WithMessage("Password is invalid.");
    }
}
