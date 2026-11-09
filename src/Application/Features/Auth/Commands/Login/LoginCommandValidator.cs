using FluentValidation;

namespace Application.Features.Auth.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        _ = RuleFor(p => p.UserName).NotNull().NotEmpty()
            .WithMessage("Username should not be null or empty.");

        _ = RuleFor(p => p.Password).NotNull().NotEmpty()
            .WithMessage("Password should not be null or empty.");
    }
}
