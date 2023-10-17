﻿namespace API.Endpoints.Account.Update;

internal sealed class Request {
    [From(Claim.AccountID, IsRequired = true)]
    public string AccountID { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

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
            .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.")
            .MinimumLength(User.MinPasswordLength).WithMessage($"Password must be at least {User.MinPasswordLength} characters.")
            .MaximumLength(User.MaxPasswordLength).WithMessage($"Password cannot be longer than {User.MaxPasswordLength} characters.");
    }
}

internal sealed class Response {
}
