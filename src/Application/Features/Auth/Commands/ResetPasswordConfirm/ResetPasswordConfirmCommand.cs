using Domain.Entities.UserAggregate;

namespace Application.Features.Auth.Commands.ResetPasswordConfirm;

public class ResetPasswordConfirmCommand : ICommandRequest<bool>
{
    public string ResetPasswordToken { get; set; } = null!;
    public string OldPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    public string ConfirmNewPassword { get; set; } = null!;
}

public class ResetPasswordConfirmCommandHandler(IAuthDbContext dbContext, UserManager<User> userManager) : CommandRequestHandler<ResetPasswordConfirmCommand, bool>
{
    public override async Task<Result<bool>> Handle(ResetPasswordConfirmCommand request, CancellationToken cancellationToken = default)
    {
        var userToken = await dbContext.UserTokens.Where(p => p.Value == request.ResetPasswordToken
                && p.ValidTo.AddMinutes(30) > DateTime.Now)
            .SingleOrDefaultAsync(cancellationToken);
        userToken.ThrowIf(p => p is null, "Invalid Token.");

        var user = await userManager.FindByIdAsync(userToken!.UserId.ToString());

        var passwordIsValid = await userManager.CheckPasswordAsync(user!, request.OldPassword);
        passwordIsValid.ThrowIf(p => p == false, "Invalid Old password.");

        var passwordResetToken = await userManager.ResetPasswordAsync(user!, request.ResetPasswordToken, request.NewPassword);

        return Ok(true);
    }
}