using GovServices.Server.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Service> Services { get; set; }
        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<WorkflowStep> WorkflowSteps { get; set; }
        public DbSet<WorkflowTransition> WorkflowTransitions { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationLog> ApplicationLogs { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OutgoingDocument> OutgoingDocuments { get; set; }
        public DbSet<OutgoingAttachment> OutgoingAttachments { get; set; }
        public DbSet<GeoObject> GeoObjects { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<ServiceTemplate> ServiceTemplates { get; set; }
        public DbSet<RosreestrRequest> RosreestrRequests { get; set; }
        public DbSet<ZagsRequest> ZagsRequests { get; set; }
        public DbSet<ZagsRequestAttachment> ZagsRequestAttachments { get; set; }
        public DbSet<RosreestrRequestAttachment> RosreestrRequestAttachments { get; set; }
        public DbSet<ExtractRequest> ExtractRequests { get; set; }
        public DbSet<SedDocumentLog> SedDocumentLogs { get; set; }
        public DbSet<ApplicationResult> ApplicationResults { get; set; }
        public DbSet<ApplicationRevision> ApplicationRevisions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<PasswordChangeLog> PasswordChangeLogs { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionGroup> PermissionGroups { get; set; }
        public DbSet<PermissionGroupPermission> PermissionGroupPermissions { get; set; }
        public DbSet<UserPermissionGroup> UserPermissionGroups { get; set; }
        public DbSet<PageAccess> PageAccesses { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Dictionary> Dictionaries { get; set; }
        public DbSet<NumberTemplate> NumberTemplates { get; set; }
        public DbSet<NumberTemplateCounter> NumberTemplateCounters { get; set; }
        public DbSet<ErrorReport> ErrorReports { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserPermissionGroup>()
                .HasKey(upg => new { upg.UserId, upg.PermissionGroupId });

            builder.Entity<UserPermissionGroup>()
                .HasOne(upg => upg.User)
                .WithMany(u => u.PermissionGroups)
                .HasForeignKey(upg => upg.UserId);

            builder.Entity<UserPermissionGroup>()
                .HasOne(upg => upg.PermissionGroup)
                .WithMany(pg => pg.UserPermissionGroups)
                .HasForeignKey(upg => upg.PermissionGroupId);

            builder.Entity<PermissionGroupPermission>()
                .HasKey(pgp => new { pgp.PermissionGroupId, pgp.PermissionId });

            builder.Entity<PermissionGroupPermission>()
                .HasOne(pgp => pgp.PermissionGroup)
                .WithMany(pg => pg.PermissionGroupPermissions)
                .HasForeignKey(pgp => pgp.PermissionGroupId);

            builder.Entity<PermissionGroupPermission>()
                .HasOne(pgp => pgp.Permission)
                .WithMany(p => p.PermissionGroupPermissions)
                .HasForeignKey(pgp => pgp.PermissionId);

            builder.Entity<PageAccess>()
                .HasOne(pa => pa.User)
                .WithMany(u => u.PageAccesses)
                .HasForeignKey(pa => pa.UserId);

            builder.Entity<Document>()
                .HasIndex(d => new { d.OwnerId, d.Type, d.CreatedAt });

            builder.Entity<Dictionary>()
                .Property(d => d.Data)
                .HasColumnType("jsonb");

            builder.Entity<NumberTemplateCounter>()
                .HasIndex(c => new { c.TemplateId, c.ScopeKey })
                .IsUnique();

            builder.Entity<Department>()
                .HasOne(d => d.ParentDepartment)
                .WithMany(d => d.ChildDepartments)
                .HasForeignKey(d => d.ParentDepartmentId);

            builder.Entity<Position>()
                .HasOne(p => p.Department)
                .WithMany(d => d.Positions)
                .HasForeignKey(p => p.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserProfile>()
                .HasOne(u => u.User)
                .WithOne()
                .HasForeignKey<UserProfile>(u => u.UserId);

            builder.Entity<UserProfile>()
                .HasOne(u => u.Position)
                .WithMany(p => p.UserProfiles)
                .HasForeignKey(u => u.PositionId);

            builder.Entity<UserProfile>()
                .HasOne(u => u.Department)
                .WithMany()
                .HasForeignKey(u => u.DepartmentId);

            builder.Entity<UserProfile>()
                .HasOne(u => u.Supervisor)
                .WithMany(s => s.Subordinates)
                .HasForeignKey(u => u.SupervisorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Service>()
                .HasOne(s => s.ResponsibleDepartment)
                .WithMany()
                .HasForeignKey(s => s.ResponsibleDepartmentId);

            builder.Entity<Permission>()
                .HasOne(p => p.Service)
                .WithMany()
                .HasForeignKey(p => p.ServiceId);

            builder.Entity<Permission>()
                .HasOne(p => p.Department)
                .WithMany()
                .HasForeignKey(p => p.DepartmentId);

            var createdAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.Entity<Department>().HasData(
                new Department { Id = SeedData.Departments.IT, Name = "IT" },
                new Department { Id = SeedData.Departments.Legal, Name = "Юридический" },
                new Department { Id = SeedData.Departments.HR, Name = "Кадры" }
            );

            builder.Entity<Service>().HasData(
                new Service
                {
                    Id = SeedData.DefaultServiceId,
                    Name = "Базовая услуга",
                    Description = "Сервис по умолчанию",
                    CreatedAt = createdAt,
                    UpdatedAt = createdAt
                }
            );

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "8fd9f63e-0001-4000-8000-000000000001",
                    Name = SeedData.Roles.Specialist,
                    NormalizedName = SeedData.Roles.Specialist.ToUpper()
                },
                new IdentityRole
                {
                    Id = "8fd9f63e-0002-4000-8000-000000000002",
                    Name = SeedData.Roles.DepartmentHead,
                    NormalizedName = SeedData.Roles.DepartmentHead.ToUpper()
                },
                new IdentityRole
                {
                    Id = "8fd9f63e-0003-4000-8000-000000000003",
                    Name = SeedData.Roles.ManagementHead,
                    NormalizedName = SeedData.Roles.ManagementHead.ToUpper()
                }
            );

            builder.Entity<Permission>().HasData(
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
        }
    }
}
