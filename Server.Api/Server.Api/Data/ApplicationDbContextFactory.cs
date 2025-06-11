using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using GovServices.Server.Data;

namespace GovServices.Server.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=gov_services;Username=postgres;Password=postgres",
            o => o.UseNetTopologySuite());
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
