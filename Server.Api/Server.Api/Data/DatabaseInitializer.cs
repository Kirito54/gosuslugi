using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GovServices.Server.Entities;

namespace GovServices.Server.Data;

public static class DatabaseInitializer
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.MigrateAsync();

        if (!context.Departments.Any())
        {
            context.Departments.AddRange(
                new Department { Id = SeedData.Departments.IT, Name = "IT" },
                new Department { Id = SeedData.Departments.Legal, Name = "Юридический" },
                new Department { Id = SeedData.Departments.HR, Name = "Кадры" }
            );
            await context.SaveChangesAsync();
        }

        foreach (var roleName in new[] { SeedData.Roles.Specialist, SeedData.Roles.DepartmentHead, SeedData.Roles.ManagementHead })
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        if (!context.Permissions.Any())
        {
            var createdAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            if (!context.Services.Any(s => s.Id == SeedData.DefaultServiceId))
            {
                context.Services.Add(new Service
                {
                    Id = SeedData.DefaultServiceId,
                    Name = "Базовая услуга",
                    Description = "Сервис по умолчанию",
                    CreatedAt = createdAt,
                    UpdatedAt = createdAt
                });
                await context.SaveChangesAsync();
            }

            context.Permissions.AddRange(
                new Permission
                {
                    Id = SeedData.Permissions.AccessApplications,
                    Name = "Доступ к заявлениям",
                    DepartmentId = SeedData.Departments.IT,
                    Role = SeedData.Roles.Specialist,
                    ServiceId = SeedData.DefaultServiceId,
                    CanView = true
                },
                new Permission
                {
                    Id = SeedData.Permissions.EditContracts,
                    Name = "Редактирование договоров",
                    DepartmentId = SeedData.Departments.Legal,
                    Role = SeedData.Roles.DepartmentHead,
                    ServiceId = SeedData.DefaultServiceId,
                    CanEdit = true
                }
            );
            await context.SaveChangesAsync();
        }
    }
}
