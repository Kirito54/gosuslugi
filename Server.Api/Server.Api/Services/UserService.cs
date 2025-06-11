using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public UserService(
        UserManager<ApplicationUser> userManager,
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
        var users = await _context.Users.Include(u => u.Department).ToListAsync();
        var result = new List<UserDto>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            result.Add(new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FullName = user.FullName!,
                DepartmentName = user.Department?.Name,
                Roles = roles.ToList()
            });
        }
        return result;
    }

    public async Task<UserDto?> GetByIdAsync(string id)
    {
        var user = await _context.Users.Include(u => u.Department)
            .FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            return null;

        var roles = await _userManager.GetRolesAsync(user);
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            FullName = user.FullName!,
            DepartmentName = user.Department?.Name,
            Roles = roles.ToList()
        };
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName,
            DepartmentId = dto.DepartmentId,
            PasswordLastChangedAt = DateTime.UtcNow
        };

        var temp = Guid.NewGuid().ToString().Substring(0, 8) + "Aa1";
        var res = await _userManager.CreateAsync(user, temp);
        if (!res.Succeeded)
            throw new Exception(string.Join("; ", res.Errors.Select(e => e.Description)));

        foreach (var roleId in dto.RoleIds)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                await _userManager.AddToRoleAsync(user, role.Name!);
            }
        }

        await _emailService.SendEmailAsync(user.Email!, "Регистрация", $"Ваш пароль: {temp}");
        return (await GetByIdAsync(user.Id))!;
    }

    public async Task<UserDto> UpdateAsync(string id, UpdateUserDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            throw new Exception("User not found");

        user.FullName = dto.FullName;
        user.DepartmentId = dto.DepartmentId;

        var currentRoles = await _userManager.GetRolesAsync(user);
        var desiredRoles = new List<string>();
        foreach (var roleId in dto.RoleIds)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                desiredRoles.Add(role.Name!);
            }
        }

        var rolesToAdd = desiredRoles.Except(currentRoles);
        var rolesToRemove = currentRoles.Except(desiredRoles);

        if (rolesToAdd.Any())
            await _userManager.AddToRolesAsync(user, rolesToAdd);
        if (rolesToRemove.Any())
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return (await GetByIdAsync(user.Id))!;
    }

    public async Task DeleteAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            await _userManager.DeleteAsync(user);
        }
    }

    public async Task ChangePasswordAsync(string id, ChangePasswordDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            throw new Exception("User not found");

        var res = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
        if (!res.Succeeded)
            throw new Exception(string.Join("; ", res.Errors.Select(e => e.Description)));

        user.PasswordLastChangedAt = DateTime.UtcNow;
        _context.Users.Update(user);
        _context.Set<PasswordChangeLog>().Add(new PasswordChangeLog
        {
            UserId = user.Id,
            Type = "Changed",
            Timestamp = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
    }
}
