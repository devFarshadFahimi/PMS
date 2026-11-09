using FluentValidation;

namespace Application.Features.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(p => p.Email).NotEmpty().NotNull();
        RuleFor(p => p.Password).NotEmpty().NotNull();
        RuleFor(p => p.ConfirmPassword).NotEmpty().NotNull().Matches(p => p.Password);
    }
}
