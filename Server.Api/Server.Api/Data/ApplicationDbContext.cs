using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GovServices.Server.Entities;

namespace GovServices.Server.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Department> Departments { get; set; } = default!;
    public DbSet<OutgoingDocument> OutgoingDocuments { get; set; } = default!;
    public DbSet<OutgoingAttachment> OutgoingAttachments { get; set; } = default!;
}
