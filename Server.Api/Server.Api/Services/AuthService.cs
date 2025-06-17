using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GovServices.Server.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<AuthResultDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
        {
            throw new UnauthorizedAccessException();
        }

        var res = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!res.Succeeded)
        {
            throw new UnauthorizedAccessException();
        }

        var jwtKey = _configuration["JwtSettings:SecretKey"];
        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            throw new Exception("JWT ключ не задан");
        }
        var key = Encoding.UTF8.GetBytes(jwtKey);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Name, user.UserName ?? string.Empty)
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var duration = 60; // токен действует 1 час

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(duration),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(descriptor);
        var jwt = handler.WriteToken(token);

        return new AuthResultDto
        {
            Token = jwt,
            RefreshToken = jwt,
            Expiration = descriptor.Expires!.Value
        };
    }

    public Task<bool> RefreshTokenAsync(string refreshToken)
    {
        return Task.FromResult(false);
    }
}
