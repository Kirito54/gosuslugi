using GovServices.Server.Interfaces;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public UserService(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context,
        IEmailService emailService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _emailService = emailService;
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await _context.Users
            .Include(u => u.Department)
            .Include(u => u.PermissionGroups)
                .ThenInclude(pg => pg.PermissionGroup)
            .ToListAsync();

        var result = new List<UserDto>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var groups = user.PermissionGroups.Select(g => g.PermissionGroup.Name).ToList();
            result.Add(new UserDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                DepartmentName = user.Department?.Name ?? string.Empty,
                Roles = roles.ToList(),
                Groups = groups
            });
        }

        return result;
    }

    public async Task<UserDto> GetByIdAsync(string id)
    {
        var user = await _context.Users
            .Include(u => u.Department)
            .Include(u => u.PermissionGroups)
                .ThenInclude(pg => pg.PermissionGroup)
            .FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new KeyNotFoundException($"User {id} not found");

        var roles = await _userManager.GetRolesAsync(user);

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FullName = user.FullName,
            DepartmentName = user.Department?.Name ?? string.Empty,
            Roles = roles.ToList(),
            Groups = user.PermissionGroups.Select(g => g.PermissionGroup.Name).ToList()
        };
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var fullName = string.Join(' ', new[] { dto.LastName, dto.FirstName, dto.MiddleName }.Where(s => !string.IsNullOrWhiteSpace(s)));
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = fullName,
            DepartmentId = dto.DepartmentId,
            PasswordLastChangedAt = DateTime.UtcNow
        };

        var temp = Guid.NewGuid().ToString().Substring(0, 8) + "Aa1";
        var res = await _userManager.CreateAsync(user, temp);
        if (!res.Succeeded)
            throw new InvalidOperationException(string.Join(";", res.Errors.Select(e => e.Description)));


        foreach (var roleId in dto.RoleIds)
        {
            var role = await _roleManager.FindByIdAsync(roleId)
                       ?? throw new KeyNotFoundException($"Role {roleId} not found");
            await _userManager.AddToRoleAsync(user, role.Name!);
        }

        foreach (var groupId in dto.GroupIds)
        {
            _context.UserPermissionGroups.Add(new UserPermissionGroup
            {
                UserId = user.Id,
                PermissionGroupId = groupId
            });
        }

        _context.UserProfiles.Add(new UserProfile
        {
            UserId = user.Id,
            FullName = fullName,
            PositionId = dto.PositionId,
            DepartmentId = dto.DepartmentId,
            IsActive = true
        });

        await _emailService.SendEmailAsync(user.Email!, "Регистрация", $"Ваш пароль: {temp}");

        return await GetByIdAsync(user.Id);
    }

    public async Task UpdateAsync(string id, UpdateUserDto dto)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id)
                   ?? throw new KeyNotFoundException($"User {id} not found");

        var fullName = string.Join(' ', new[] { dto.LastName, dto.FirstName, dto.MiddleName }.Where(s => !string.IsNullOrWhiteSpace(s)));
        user.FullName = fullName;
        user.DepartmentId = dto.DepartmentId;

        var currentRoles = await _userManager.GetRolesAsync(user);
        var desiredRoles = new List<string>();
        foreach (var roleId in dto.RoleIds)
        {
            var role = await _roleManager.FindByIdAsync(roleId)
                       ?? throw new KeyNotFoundException($"Role {roleId} not found");
            desiredRoles.Add(role.Name!);
        }

        var rolesToAdd = desiredRoles.Except(currentRoles).ToList();
        var rolesToRemove = currentRoles.Except(desiredRoles).ToList();

        if (rolesToAdd.Any())
            await _userManager.AddToRolesAsync(user, rolesToAdd);

        if (rolesToRemove.Any())
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

        var existingGroups = _context.UserPermissionGroups.Where(g => g.UserId == id).ToList();
        _context.UserPermissionGroups.RemoveRange(existingGroups);
        foreach (var groupId in dto.GroupIds)
        {
            _context.UserPermissionGroups.Add(new UserPermissionGroup
            {
                UserId = id,
                PermissionGroupId = groupId
            });
        }

        var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == id);
        if (profile != null)
        {
            profile.FullName = fullName;
            profile.PositionId = dto.PositionId;
            profile.DepartmentId = dto.DepartmentId;
            _context.UserProfiles.Update(profile);
        }

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id)
                   ?? throw new KeyNotFoundException($"User {id} not found");

        await _userManager.DeleteAsync(user);
    }

    public async Task ChangePasswordAsync(string id, ChangePasswordDto dto)
    {
        var user = await _userManager.FindByIdAsync(id)
                   ?? throw new KeyNotFoundException($"User {id} not found");

        var res = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
        if (!res.Succeeded)
            throw new InvalidOperationException(string.Join(";", res.Errors.Select(e => e.Description)));

        user.PasswordLastChangedAt = DateTime.UtcNow;
        _context.Users.Update(user);
        _context.PasswordChangeLogs.Add(new PasswordChangeLog
        {
            UserId = user.Id,
            Type = "Changed",
            Timestamp = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
    }
}
