namespace API.Endpoints.Users.Signup;

internal sealed class Request {
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

internal sealed class Validator : Validator<Request> {
    private int MaxNameLength;
    private int MinPasswordLength;
    private int MaxPasswordLength;
    public Validator() {
        MaxNameLength = 50;
        MinPasswordLength = 10;
        MaxPasswordLength = 70;

        RuleFor(user => user.DisplayName)
            .NotEmpty().WithMessage("DisplayName is required.")
            .MinimumLength(1).WithMessage("DisplayName cannot be empty.")
            .MaximumLength(MaxNameLength).WithMessage($"DisplayName cannot be longer than {MaxNameLength} characters.");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("The format of your email address is invalid.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.")
            .MinimumLength(MinPasswordLength).WithMessage($"Password must be at least {MinPasswordLength} characters.")
            .MaximumLength(MaxPasswordLength).WithMessage($"Password cannot be longer than {MaxPasswordLength} characters.");
    }
}

internal sealed class Response {
    public string Message { get; set; }
}
