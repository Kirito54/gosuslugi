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
    public DbSet<Service> Services { get; set; } = default!;
    public DbSet<GeoObject> GeoObjects { get; set; } = default!;
    public DbSet<PasswordChangeLog> PasswordChangeLogs { get; set; } = default!;
}
