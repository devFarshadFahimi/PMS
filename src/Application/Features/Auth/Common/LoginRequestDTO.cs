namespace Application.Features.Auth.Common;

public class LoginRequestDTO
{
    public required long Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
}
