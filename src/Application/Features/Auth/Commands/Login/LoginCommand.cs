using Domain.Entities.UserAggregate;

namespace Application.Features.Auth.Commands.Login;

public class LoginCommand : ICommandRequest<LoginResponseDTO>
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}

public class LoginCommandHandler : CommandRequestHandler<LoginCommand, LoginResponseDTO>
{
    private readonly IAuthService _authService;
    private readonly UserManager<User> _userManager;
    private readonly IAuthDbContext _dbContext;
    private readonly ICacheAdapter _cacheAdapter;
    private readonly IDateTimeService _dateTimeService;

    public LoginCommandHandler(
        IAuthService authService,
        UserManager<User> userManager,
        IAuthDbContext dbContext,
        ICacheAdapter cacheAdapter,
        IDateTimeService dateTimeService)
    {
        _authService = authService;
        _userManager = userManager;
        _dbContext = dbContext;
        _cacheAdapter = cacheAdapter;
        _dateTimeService = dateTimeService;
    }

    public override async Task<Result<LoginResponseDTO>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users.Where(p => p.UserName == request.UserName)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Error("Invalid credential..., please check your username and password.");
        }

        var passwordIsValid = await _userManager.CheckPasswordAsync(user!, request.Password);
        if (!passwordIsValid)
        {
            var result = await _userManager.AccessFailedAsync(user!);
            if (!result.Succeeded)
            {
                // log the result error.
            }

            return Error("Invalid credential...!");
        }

        var response = _authService.GetAuthToken(new LoginRequestDTO
        {
            Id = user!.Id,
            Email = user.Email!,
            UserName = user.UserName!,
            PhoneNumber = user.PhoneNumber,
        });

        _ = await _dbContext.UserTokens.AddAsync(
            new UserToken
            {
                LoginProvider = "Swagger",
                UserId = user.Id,
                Name = "RefreshToken",
                Value = response.RefreshToken,
                ValidTo = response.RefreshTokenExpiryTime
            }, cancellationToken);
        await _dbContext.SaveChangeAsync();

        _cacheAdapter.Add(user.Id + string.Empty, response, _dateTimeService.UtcNow.AddMinutes(10), null);
        return await OkAsync(response);
    }
}
