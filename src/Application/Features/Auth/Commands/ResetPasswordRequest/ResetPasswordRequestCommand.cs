using System.Security.Cryptography;
using Domain.Entities.UserAggregate;

namespace Application.Features.Auth.Commands.ResetPasswordRequest;

public class ResetPasswordRequestCommand : ICommandRequest<string>
{
    public string Email { get; set; } = null!;
    public string EmailToUpper => Email.ToUpper();
}

public class ResetPasswordRequestCommandHandler(UserManager<User> userManager, IAuthDbContext dbContext) : CommandRequestHandler<ResetPasswordRequestCommand, string>
{
    public override async Task<Result<string>> Handle(ResetPasswordRequestCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            //var user = await dbContext.Users.Where(p => p.NormalizedEmail == request.EmailToUpper).SingleOrDefaultAsync(cancellationToken);

            user.ThrowIf(p => p is null, "Invalid email address.");

            var passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(user!);
            //var passwordResetToken = GenerateRefreshToken();

            _ = await dbContext.UserTokens.AddAsync(
                new UserToken
                {
                    LoginProvider = "SYSTEM",
                    UserId = user!.Id,
                    Name = "PasswordResetToken",
                    Value = passwordResetToken,
                    ValidTo = DateTime.Now.AddMinutes(30)
                }, cancellationToken);
            await dbContext.SaveChangeAsync(cancellationToken);
            return Ok(passwordResetToken);
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
            throw;
        }
    }
    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}