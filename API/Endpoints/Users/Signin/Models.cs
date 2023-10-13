using API.Auth;

namespace API.Endpoints.Users.Signin;

internal sealed class SigninReq {
    public string Email { get; set; }
    public string Password { get; set; }
}

internal sealed class Validator : Validator<SigninReq> {
    private int MinPasswordLength;
    private int MaxPasswordLength;
    public Validator() {
        MinPasswordLength = 10;
        MaxPasswordLength = 70;

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("The format of your email address is invalid.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(MinPasswordLength).WithMessage($"Password must be at least {MinPasswordLength} characters.")
            .MaximumLength(MaxPasswordLength).WithMessage($"Password cannot be longer than {MaxPasswordLength} characters.");
    }
}

internal sealed class SigninRes {
    public string Message { get; set; }
    public JwtToken Token { get; set; } = new JwtToken();

}
