using System.Text.RegularExpressions;
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

public class Tes2tController(IMediator mediator) : ApiControllerBase(mediator)
{
    public static readonly string[] InvalidNationalCodes =
    {
		// "1111111111", // این کد جدیداً مشخص شده است که قابل قبول می باشد
		"0000000000", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777",
        "8888888888", "9999999999"
    };

    public static bool NationalCodeValidation(string nationalCode, out string result, bool isForigenPersonAccepted = true)
    {
        // در صورتی که کد ملی وارد شده تهی باشد
        if (string.IsNullOrEmpty(nationalCode))
        {
            result = "خطا در ارسال متن خالی.";
            return false;
        }

        // در صورتی که کد ملی ده رقم عددی نباشد
        var regex = new Regex(@"[0-9]{10}");
        if (!regex.IsMatch(nationalCode))
        {
            result = "کد ملی باید 10 رقمی باشد.";
            return false;
        }

        // -> اتباع بیگانه
        if (nationalCode.StartsWith("9") && !isForigenPersonAccepted)
        {
            result = "کد ملی متعلق به اتباع خارجی بوده و معتبر نمی باشد.";
            return false;
        }

        if (InvalidNationalCodes.Contains(nationalCode))
        {
            result = "کد ملی اعداد تکراری نمی تواند باشد.";
            return false;
        }

        // عملیات اعتبار سنجی با الگوریتم
        var sum = nationalCode.Select((c, i) => c * (10 - i)).Sum();
        var check = (int)nationalCode[9];
        var net = (sum - check) % 11;
        var checkNet = net < 2 ? net : 11 - net;

        if (checkNet == check)
        {
            result = "کد ملی معتبر می باشد.";
            return true;
        }

        result = "کد ملی معتبر نمی باشد.";
        return false;
    }

    [HttpPost(nameof(Test))]
    public async Task<IActionResult> Test(string nationalCode, CancellationToken cancellationToken)
    {
        _ = NationalCodeValidation(nationalCode, out string result);
        return Ok(result);
    }
}

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
