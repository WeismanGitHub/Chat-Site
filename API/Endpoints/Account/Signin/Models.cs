using API.Auth;

namespace API.Endpoints.Account.Signin;

internal sealed class Request {
    public string Email { get; set; }
    public string Password { get; set; }
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
