using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services;

public class PermissionService : IPermissionService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PermissionService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PermissionDto>> GetAllAsync()
    {
        var entities = await _context.Permissions.ToListAsync();
        return _mapper.Map<List<PermissionDto>>(entities);
    }
}
