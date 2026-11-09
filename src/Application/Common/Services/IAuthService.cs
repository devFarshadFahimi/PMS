namespace Application.Common.Services;
public interface IAuthService
{
    LoginResponseDTO GetAuthToken(LoginRequestDTO user);
}
