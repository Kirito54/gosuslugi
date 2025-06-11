namespace GovServices.Server.Interfaces;

using GovServices.Server.DTOs;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(string id);
    Task<UserDto> CreateAsync(CreateUserDto dto);
    Task<UserDto> UpdateAsync(string id, UpdateUserDto dto);
    Task DeleteAsync(string id);
    Task ChangePasswordAsync(string id, ChangePasswordDto dto);
}
