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
        public DbSet<DocumentMetadata> DocumentMetadatas { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OutgoingDocument> OutgoingDocuments { get; set; }
        public DbSet<OutgoingAttachment> OutgoingAttachments { get; set; }
        public DbSet<GeoObject> GeoObjects { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<RosreestrRequest> RosreestrRequests { get; set; }
        public DbSet<SedDocumentLog> SedDocumentLogs { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<PasswordChangeLog> PasswordChangeLogs { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // здесь можно добавить конфигурацию связей и ограничений, если надо
        }
    }
}
