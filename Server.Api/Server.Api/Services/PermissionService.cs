using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Interfaces;
using GovServices.Server.Entities;
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

    public async Task<PermissionDto> CreateAsync(CreatePermissionDto dto)
    {
        var entity = _mapper.Map<Permission>(dto);
        _context.Permissions.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<PermissionDto>(entity);
    }

    public async Task UpdateAsync(int id, UpdatePermissionDto dto)
    {
        var entity = await _context.Permissions.FindAsync(id) ?? throw new KeyNotFoundException();
        _mapper.Map(dto, entity);
        await _context.SaveChangesAsync();
    }
}
