using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GovServices.Server.Entities;
using GovServices.Server.Authorization;
using GovServices.Server.Services.Numbering;
using System;
using System.Linq;

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
                RoleNames.Specialist,
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

            var defaultDepartmentId = SeedData.Departments.IT;
            if (!context.Departments.Any(d => d.Id == defaultDepartmentId))
            {
                context.Departments.Add(new Department { Id = defaultDepartmentId, Name = "IT" });
                await context.SaveChangesAsync();
            }

            if (!context.Services.Any(s => s.Id == SeedData.DefaultServiceId))
            {
                context.Services.Add(new Service
                {
                    Id = SeedData.DefaultServiceId,
                    Name = "Базовая услуга",
                    Description = "Сервис по умолчанию",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
                await context.SaveChangesAsync();
            }

            foreach (var perm in permissions)
            {
                if (!context.Permissions.Any(p => p.Name == perm))
                {
                    context.Permissions.Add(new Permission
                    {
                        Name = perm,
                        DepartmentId = defaultDepartmentId,
                        ServiceId = SeedData.DefaultServiceId,
                        Role = SeedData.Roles.Specialist
                    });
                }
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

            if (!context.NumberTemplates.Any(t => t.TargetType == "Application"))
            {
                context.NumberTemplates.Add(new NumberTemplate
                {
                    Name = "Application",
                    TargetType = "Application",
                    TemplateText = "{{Year}}/{{Sequential:3}}",
                    ResetPolicy = ResetPolicy.Yearly
                });
                await context.SaveChangesAsync();
            }

            if (!context.Workflows.Any(w => w.Id == SeedData.DefaultServiceId))
            {
                var workflow = new Workflow
                {
                    Id = SeedData.DefaultServiceId,
                    Name = "Базовый процесс",
                    Description = "Процесс по умолчанию"
                };
                context.Workflows.Add(workflow);
                await context.SaveChangesAsync();

                var step1 = new WorkflowStep { WorkflowId = workflow.Id, Name = "Прием", Sequence = 1 };
                var step2 = new WorkflowStep { WorkflowId = workflow.Id, Name = "Завершено", Sequence = 2 };
                context.WorkflowSteps.AddRange(step1, step2);
                await context.SaveChangesAsync();

                context.WorkflowTransitions.Add(new WorkflowTransition { FromStepId = step1.Id, ToStepId = step2.Id });
                await context.SaveChangesAsync();
            }

            if (!context.Applications.Any())
            {
                var generator = new NumberGenerator(context);
                var number = await generator.GenerateAsync("Application");

                var firstStep = context.WorkflowSteps
                    .Where(s => s.WorkflowId == SeedData.DefaultServiceId)
                    .OrderBy(s => s.Sequence)
                    .FirstOrDefault();

                if (firstStep == null)
                {
                    var step1 = new WorkflowStep { WorkflowId = SeedData.DefaultServiceId, Name = "Прием", Sequence = 1 };
                    var step2 = new WorkflowStep { WorkflowId = SeedData.DefaultServiceId, Name = "Завершено", Sequence = 2 };
                    context.WorkflowSteps.AddRange(step1, step2);
                    await context.SaveChangesAsync();

                    context.WorkflowTransitions.Add(new WorkflowTransition { FromStepId = step1.Id, ToStepId = step2.Id });
                    await context.SaveChangesAsync();

                    firstStep = step1;
                }
                var application = new Application
                {
                    Number = number,
                    ServiceId = SeedData.DefaultServiceId,
                    CurrentStepId = firstStep.Id,
                    Status = "New",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Source = ApplicationSource.Mfc,
                    ApplicantName = "Тестовый заявитель",
                    Address = "г. Москва, ул. Пушкина, д. 1",
                    RegistrarId = admin.Id,
                    AssignedToUserId = admin.Id
                };
                context.Applications.Add(application);
                await context.SaveChangesAsync();
            }
        }
    }
}
