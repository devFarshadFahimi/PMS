using System.Security.Claims;

namespace Application.Features.Auth.Queries.GetUserInfo;

public class UserInfoResponse
{
    public string Id { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = [];
    public List<string> HomePhones { get; set; } = [];
}

public class GetUserInfoQuery : IQueryRequest<UserInfoResponse>
{
    public ClaimsPrincipal User { get; set; } = null!;
}

public class GetUserInfoQueryHandler : QueryRequestHandler<GetUserInfoQuery, UserInfoResponse>
{
    public override Task<Result<UserInfoResponse>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        if (Random.Shared.Next(0, 10) < 5)
        {
            return ErrorAsync("پروفایل کاربر رو نمیدممممم");
        }

        var user = new UserInfoResponse
        {
            Id = request.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? request.User.FindFirstValue("sub") ?? string.Empty,
            Nickname = request.User.FindFirstValue("nickname") ?? string.Empty,
            Email = request.User.FindFirstValue(ClaimTypes.Email) ?? request.User.FindFirstValue("email") ?? string.Empty,
            PhoneNumber = request.User.FindFirstValue(ClaimTypes.MobilePhone) ?? request.User.FindFirstValue("phone_number") ?? string.Empty,
            Roles = [.. request.User.Claims
                .Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                .SelectMany(c => c.Value.Split(',').ToList())],
            HomePhones = [.. request.User.Claims
                .Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/homephone")
                .SelectMany(c => c.Value.Split(',').ToList())]
        };

        return OkAsync(user);
    }
}