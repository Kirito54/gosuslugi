namespace GovServices.Server.Interfaces;

using GovServices.Server.DTOs;

public interface IAuthService
{
    Task<AuthResultDto> LoginAsync(LoginRequestDto dto);
    Task<bool> RefreshTokenAsync(string refreshToken);
}
