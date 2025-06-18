using GovServices.Server.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        public DbSet<Dictionary> Dictionaries { get; set; }

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

            builder.Entity<Document>()
                .HasIndex(d => new { d.OwnerId, d.Type, d.CreatedAt });

            builder.Entity<Dictionary>()
                .Property(d => d.Data)
                .HasColumnType("jsonb");
        }
    }
}
