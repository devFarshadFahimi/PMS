namespace Application.Features.Auth.Common;

public class LoginResponseDTO(
    string accessToken,
    string refreshToken,
    DateTime accessTokenExpiryTime,
    DateTime refreshTokenExpiryTime)
{
    public string AccessToken { get; private set; } = accessToken;
    public string RefreshToken { get; private set; } = refreshToken;
    public DateTime AccessTokenExpiryTime { get; private set; } = accessTokenExpiryTime;
    public DateTime RefreshTokenExpiryTime { get; private set; } = refreshTokenExpiryTime;
}