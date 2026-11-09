using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Services;
using Application.Features.Auth.Common;
using BusinessMakerFramework.Application.Service.Contracts;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

/// <summary>
/// Creates signed JWT access tokens and random refresh tokens using the configured token settings.
/// </summary>
public class AuthService(IOptions<AuthTokenOption> authTokenConfig, IDateTimeService dateTimeService) : IAuthService
{
    private readonly AuthTokenOption _tokenConfig = authTokenConfig.Value;

    /// <summary>
    /// Creates an access token and refresh token for the supplied user, including the predefined role and home-phone claims.
    /// </summary>
    /// <param name="user">The user whose identity claims are included in the access token.</param>
    /// <returns>The access token, refresh token, and their expiration times.</returns>
    public LoginResponseDTO GetAuthToken(LoginRequestDTO user)
    {
        var claims = new List<Claim>()
        {
                new ("roles", "Admin"),
                new ("roles", "Customer"),
                new ("roles", "User"),
                new ("homePhones", "02133875411"),
                new ("homePhones", "02133875065")
        };
        var authClaims = GetClaims(user, claims);

        var token = GetToken(authClaims);
        return token;
    }

    #region Privates

    /// <summary>
    /// Generates a Base64-encoded refresh token from 64 cryptographically random bytes.
    /// </summary>
    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    /// <summary>
    /// Builds the user's identity claims and appends any additional claims supplied by the caller.
    /// </summary>
    private static List<Claim> GetClaims(LoginRequestDTO user, IList<Claim>? claims = null)
    {
        var result = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Nickname, user.UserName),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.PhoneNumber, user.PhoneNumber ?? string.Empty),
        };

        if (claims != null && claims.Any())
        {
            result.AddRange(claims);
        }

        return result;
    }

    /// <summary>
    /// Serializes a signed access token and pairs it with a refresh token that expires one day after the access token.
    /// </summary>
    private LoginResponseDTO GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(GetEncodedSecret());
        var token = GetSecurityToken(authClaims, authSigningKey);
        return new LoginResponseDTO(
            new JwtSecurityTokenHandler().WriteToken(token),
            GenerateRefreshToken(),
            token.ValidTo,
            token.ValidTo.AddDays(1));
    }

    /// <summary>
    /// Builds an HMAC-SHA256 JWT with the supplied claims and configured issuer, audience, and lifetime.
    /// </summary>
    private JwtSecurityToken GetSecurityToken(List<Claim> authClaims, SecurityKey authSigningKey)
    {
        var header = new JwtHeader(new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
        var payload = new JwtPayload(
            _tokenConfig.ValidIssuer,
            _tokenConfig.ValidAudience,
            authClaims,
            dateTimeService.UtcNow,
            dateTimeService.UtcNow.AddMinutes(_tokenConfig.TokenExpireMinutes),
            dateTimeService.UtcNow);
        return new JwtSecurityToken(header, payload);
    }

    /// <summary>
    /// Encodes the configured signing key as UTF-8 bytes.
    /// </summary>
    private byte[] GetEncodedSecret()
    {
        return Encoding.UTF8.GetBytes(_tokenConfig.SigningKey);
    }

    #endregion
}
