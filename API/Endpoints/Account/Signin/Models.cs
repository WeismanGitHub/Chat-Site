using API.Auth;

namespace API.Endpoints.Account.Signin;

internal sealed class Request {
    public string Email { get; set; }
    public string Password { get; set; }
}

internal sealed class Validator : Validator<Request> {
    private int MinPasswordLength;
    private int MaxPasswordLength;
    public Validator() {
        MinPasswordLength = 10;
        MaxPasswordLength = 70;

        RuleFor(account => account.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("The format of your email address is invalid.");

        RuleFor(account => account.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(MinPasswordLength).WithMessage($"Password must be at least {MinPasswordLength} characters.")
            .MaximumLength(MaxPasswordLength).WithMessage($"Password cannot be longer than {MaxPasswordLength} characters.");
    }
}

internal sealed class Response {
    public JwtToken Token { get; set; } = new JwtToken();

}
