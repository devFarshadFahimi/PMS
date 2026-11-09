namespace Application.Features.Auth.Commands.InvalidLoggedInToken;

public class InvalidLoggedInTokenCommand(long userId) : ICommandRequest<bool>
{
    public long UserId { get; } = userId;
}

public class InvalidLoggedInTokenCommandHandler(ICacheAdapter cacheAdapter)
    : CommandRequestHandler<InvalidLoggedInTokenCommand, bool>
{
    public override Task<Result<bool>> Handle(InvalidLoggedInTokenCommand request, CancellationToken cancellationToken = default)
    {
        cacheAdapter.RemoveCache(request.UserId + string.Empty);
        return OkAsync(true);
    }
}