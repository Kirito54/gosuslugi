using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services;

public class PermissionGroupService : IPermissionGroupService
{
    private readonly ApplicationDbContext _context;

    public PermissionGroupService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PermissionGroupDto>> GetAllAsync()
    {
        return await _context.PermissionGroups
            .Include(g => g.PermissionGroupPermissions)
                .ThenInclude(p => p.Permission)
            .Select(g => new PermissionGroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Permissions = g.PermissionGroupPermissions.Select(p => p.Permission.Name).ToList()
            })
            .ToListAsync();
    }

    public async Task<PermissionGroupDto> CreateAsync(CreatePermissionGroupDto dto)
    {
        var group = new PermissionGroup { Name = dto.Name };
        _context.PermissionGroups.Add(group);
        await _context.SaveChangesAsync();

        foreach (var pid in dto.PermissionIds)
        {
            _context.PermissionGroupPermissions.Add(new PermissionGroupPermission
            {
                PermissionGroupId = group.Id,
                PermissionId = pid
            });
        }
        await _context.SaveChangesAsync();

        return new PermissionGroupDto
        {
            Id = group.Id,
            Name = group.Name,
            Permissions = await _context.PermissionGroupPermissions
                .Where(p => p.PermissionGroupId == group.Id)
                .Include(p => p.Permission)
                .Select(p => p.Permission.Name)
                .ToListAsync()
        };
    }

    public async Task UpdateAsync(int id, UpdatePermissionGroupDto dto)
    {
        var group = await _context.PermissionGroups.FindAsync(id) ?? throw new KeyNotFoundException();
        group.Name = dto.Name;
        var existing = _context.PermissionGroupPermissions.Where(p => p.PermissionGroupId == id).ToList();
        _context.PermissionGroupPermissions.RemoveRange(existing);
        foreach (var pid in dto.PermissionIds)
        {
            _context.PermissionGroupPermissions.Add(new PermissionGroupPermission
            {
                PermissionGroupId = id,
                PermissionId = pid
            });
        }
        await _context.SaveChangesAsync();
    }
}
