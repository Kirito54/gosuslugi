using System.Threading.Tasks;
using GovServices.Server.Data;
using GovServices.Server.Entities;
using GovServices.Server.Services.Numbering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace Server.Tests;

public class NumberGeneratorTests
{
    [Fact]
    public async Task Sequential_Increments()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("nums1").Options;
        await using var db = new ApplicationDbContext(options);
        db.NumberTemplates.Add(new NumberTemplate { Id = 1, Name = "App", TargetType = "Application", TemplateText = "{{Year}}/{{Sequential:3}}", ResetPolicy = ResetPolicy.Yearly });
        await db.SaveChangesAsync();

        var generator = new NumberGenerator(db);
        var n1 = await generator.GenerateAsync("Application");
        var n2 = await generator.GenerateAsync("Application");

        Assert.Equal($"{DateTime.UtcNow:yyyy}/001", n1);
        Assert.Equal($"{DateTime.UtcNow:yyyy}/002", n2);
    }

    [Fact]
    public async Task Reset_By_Year()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("nums2").Options;
        await using var db = new ApplicationDbContext(options);
        db.NumberTemplates.Add(new NumberTemplate { Id = 1, Name = "Order", TargetType = "Order", TemplateText = "{{Year}}-{{Sequential}}", ResetPolicy = ResetPolicy.Yearly });
        await db.SaveChangesAsync();

        var generator = new NumberGenerator(db);
        var now = DateTime.UtcNow;
        var n1 = await generator.GenerateAsync("Order");

        db.NumberTemplateCounters.First().ScopeKey = (now.Year - 1).ToString();
        await db.SaveChangesAsync();

        var n2 = await generator.GenerateAsync("Order");

        Assert.EndsWith("1", n2); // counter reset
    }
}
