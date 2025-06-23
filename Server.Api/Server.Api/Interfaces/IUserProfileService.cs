using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IUserProfileService
{
    Task<List<UserProfileDto>> GetAllAsync();
}
