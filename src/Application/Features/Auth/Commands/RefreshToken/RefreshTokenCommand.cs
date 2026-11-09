using Domain.Entities.UserAggregate;

namespace Application.Features.Auth.Commands.RefreshToken;
public class RefreshTokenCommand : ICommandRequest<LoginResponseDTO>
{
    public string RefreshToken { get; set; } = null!;
}


public class RefreshTokenCommandHandler(IAuthService authService, IAuthDbContext dbContext) : CommandRequestHandler<RefreshTokenCommand, LoginResponseDTO>
{
    public override async Task<Result<LoginResponseDTO>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken = default)
    {
        var userToken = await dbContext.UserTokens
            .Where(p => p.Value == request.RefreshToken)
            .Include(p => p.User)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new Exception("Invalid refresh token.");

        if (DateTime.Now > userToken.ValidTo)
        {
            throw new UnauthorizedAccessException();
        }

        var user = userToken.User;
        var response = authService.GetAuthToken(new LoginRequestDTO
        {
            Id = user.Id,
            Email = user.Email!,
            UserName = user.UserName!,
            PhoneNumber = user.PhoneNumber,
        });

        _ = await dbContext.UserTokens.AddAsync(
            new UserToken
            {
                LoginProvider = "SYSTEM",
                UserId = user.Id,
                Name = "RefreshToken",
                Value = response.RefreshToken,
                ValidTo = response.RefreshTokenExpiryTime
            }, cancellationToken);

        await dbContext.SaveChangeAsync(cancellationToken);
        return Ok(response);
    }
}
