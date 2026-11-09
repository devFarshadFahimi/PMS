namespace Application.Features.Auth.Queries.UserIsLogin;

public class UserIsLoginQuery : IQueryRequest<bool>
{
    public string UserId { get; }

    public UserIsLoginQuery(string userId)
    {
        UserId = userId;
    }
}

public class UserIsLoginQueryHandler(ICacheAdapter cacheAdapter, IDateTimeService dateTimeService) : QueryRequestHandler<UserIsLoginQuery, bool>
{
    public override Task<Result<bool>> Handle(UserIsLoginQuery request, CancellationToken cancellationToken)
    {
        var response = cacheAdapter.Get<LoginResponseDTO?>(request.UserId);
        return response is null
            ? OkAsync(false)
            : OkAsync(response.AccessTokenExpiryTime > dateTimeService.UtcNow);
    }
}