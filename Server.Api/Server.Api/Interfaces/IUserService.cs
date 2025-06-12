using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto> GetByIdAsync(string id);
    Task<UserDto> CreateAsync(CreateUserDto dto);
    Task UpdateAsync(string id, UpdateUserDto dto);
    Task DeleteAsync(string id);
    Task ChangePasswordAsync(string id, ChangePasswordDto dto);
}
