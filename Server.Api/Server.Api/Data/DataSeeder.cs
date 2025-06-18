using Microsoft.AspNetCore.Identity;
using GovServices.Server.Entities;
using GovServices.Server.Authorization;

namespace GovServices.Server.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var context = services.GetRequiredService<ApplicationDbContext>();

            var roles = new[]
            {
                RoleNames.Administrator,
                RoleNames.Registrar,
                RoleNames.Analyst,
                RoleNames.Lawyer,
                RoleNames.DepartmentHead,
                RoleNames.ManagementHead,
                RoleNames.Director,
                RoleNames.FirstDeputyDirector,
                RoleNames.Executor,
                RoleNames.Chancery,
                RoleNames.Egrn,
                RoleNames.Vis,
                RoleNames.Zags,
                RoleNames.DocumentUpload,
                RoleNames.DocumentSign,
                RoleNames.Rdz,
                RoleNames.Rdi,
                RoleNames.ClosedServices
            };
            const string adminEmail = "admin@gosuslugi.local";
            const string adminPassword = "P@ssw0rd123";
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var permissions = new[]
            {
                PermissionNames.AccessRDZ,
                PermissionNames.AccessRDI,
                PermissionNames.RequestEGRN,
                PermissionNames.RequestVIS,
                PermissionNames.RequestZAGS,
                PermissionNames.UploadDocuments,
                PermissionNames.SignDocuments,
                PermissionNames.ViewSpecificServices,
                PermissionNames.ViewClosedServices
            };

            foreach (var perm in permissions)
            {
                if (!context.Permissions.Any(p => p.Name == perm))
                    context.Permissions.Add(new Permission { Name = perm });
            }
            await context.SaveChangesAsync();

            var fullGroup = context.PermissionGroups.FirstOrDefault(g => g.Name == "Полный доступ");
            if (fullGroup == null)
            {
                fullGroup = new PermissionGroup { Name = "Полный доступ" };
                context.PermissionGroups.Add(fullGroup);
                await context.SaveChangesAsync();

                foreach (var perm in context.Permissions)
                {
                    context.PermissionGroupPermissions.Add(new PermissionGroupPermission
                    {
                        PermissionGroupId = fullGroup.Id,
                        PermissionId = perm.Id
                    });
                }
                await context.SaveChangesAsync();
            }

            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Системный Администратор",
                    EmailConfirmed = true,
                    PasswordLastChangedAt = DateTime.UtcNow
                };
                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, RoleNames.Administrator);
                }
            }

            if (!context.UserPermissionGroups.Any(upg => upg.UserId == admin.Id && upg.PermissionGroupId == fullGroup.Id))
            {
                context.UserPermissionGroups.Add(new UserPermissionGroup
                {
                    UserId = admin.Id,
                    PermissionGroupId = fullGroup.Id
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
