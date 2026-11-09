using System.Text.Json.Serialization;
using Domain.Entities.UserAggregate;
using Mapster;

namespace Application.Features.Auth.Commands.Register;

public class RegisterCommand : ICommandRequest<bool>
{
    [JsonIgnore]
    public string? UserName => Email;
    public string? PhoneNumber { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}

public class RegisterCommandHandler(UserManager<User> userManager) : CommandRequestHandler<RegisterCommand, bool>
{
    public override async Task<Result<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken = default)
    {
        var identityResult = await userManager.CreateAsync(request.Adapt<User>(), request.Password);
        return identityResult.Succeeded
            ? await OkAsync(true)
            : await ErrorAsync(string.Join(", \n", identityResult.Errors.Select(q => q.Description).ToList()));
    }
}
