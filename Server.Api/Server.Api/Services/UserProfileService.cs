using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services;

public class UserProfileService : IUserProfileService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UserProfileService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<UserProfileDto>> GetAllAsync()
    {
        var entities = await _context.UserProfiles.ToListAsync();
        return _mapper.Map<List<UserProfileDto>>(entities);
    }
}
