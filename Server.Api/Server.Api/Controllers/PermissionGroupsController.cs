using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using System.Collections.Generic;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionGroupsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PermissionGroupsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<PermissionGroupDto>> Get()
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
}
