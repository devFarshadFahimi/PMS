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

public class AuthService(IOptions<AuthTokenOption> authTokenConfig, IDateTimeService dateTimeService) : IAuthService
{
    private readonly AuthTokenOption _tokenConfig = authTokenConfig.Value;

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

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

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

    private byte[] GetEncodedSecret()
    {
        return Encoding.UTF8.GetBytes(_tokenConfig.SigningKey);
    }

    #endregion
}