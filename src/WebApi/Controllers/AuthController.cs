using Application.Features.Auth.Commands.InvalidLoggedInToken;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.RefreshToken;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.ResetPasswordConfirm;
using Application.Features.Auth.Commands.ResetPasswordRequest;
using Application.Features.Auth.Queries.GetUserInfo;
using BusinessMakerFramework.SourceGenerator.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class AuthController(IMediator mediator) : ApiControllerBase(mediator)
{
    [HttpPost(nameof(LoginAsync))]
    public async Task<IActionResult> LoginAsync([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPost(nameof(RegisterAsync))]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPost(nameof(RefreshTokenAsync))]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPost(nameof(ResetPasswordRequestAsync))]
    public async Task<IActionResult> ResetPasswordRequestAsync([FromBody] ResetPasswordRequestCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPost(nameof(ResetPasswordConfirmAsync))]
    public async Task<IActionResult> ResetPasswordConfirmAsync([FromBody] ResetPasswordConfirmCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPost(nameof(InvalidLoggedInTokenAsync))]
    public async Task<IActionResult> InvalidLoggedInTokenAsync([FromBody] InvalidLoggedInTokenCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpGet(nameof(UserInfoAsync))]
    [AllowAnonymous]
    public async Task<IActionResult> UserInfoAsync(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetUserInfoQuery() { User = User }, cancellationToken);
    }
}
