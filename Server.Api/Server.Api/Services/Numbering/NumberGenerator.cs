using System.Text.RegularExpressions;
using GovServices.Server.Data;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services.Numbering;

public class NumberGenerator : INumberGenerator
{
    private readonly ApplicationDbContext _db;

    public NumberGenerator(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<string> GenerateAsync(string targetType)
    {
        var template = await _db.NumberTemplates.FirstOrDefaultAsync(t => t.TargetType == targetType)
                      ?? throw new KeyNotFoundException($"Template for {targetType} not found");

        var now = DateTime.UtcNow;
        string scope = template.ResetPolicy switch
        {
            ResetPolicy.Daily => now.ToString("yyyyMMdd"),
            ResetPolicy.Monthly => now.ToString("yyyyMM"),
            ResetPolicy.Yearly => now.ToString("yyyy"),
            _ => string.Empty
        };

        var counter = await _db.NumberTemplateCounters.FirstOrDefaultAsync(c => c.TemplateId == template.Id && c.ScopeKey == scope);
        if (counter == null)
        {
            counter = new NumberTemplateCounter { TemplateId = template.Id, ScopeKey = scope, CurrentValue = 0 };
            _db.NumberTemplateCounters.Add(counter);
        }
        counter.CurrentValue++;
        await _db.SaveChangesAsync();

        var result = template.TemplateText;
        result = result.Replace("{{Year}}", now.Year.ToString());
        result = result.Replace("{{Month}}", now.Month.ToString("D2"));
        result = result.Replace("{{Day}}", now.Day.ToString("D2"));
        result = Regex.Replace(result, "\\{\\{Sequential(?::(?<len>\\d+))?\\}}", m =>
        {
            var lenGroup = m.Groups["len"].Value;
            int len = 0;
            if (!string.IsNullOrEmpty(lenGroup)) int.TryParse(lenGroup, out len);
            return len > 0 ? counter.CurrentValue.ToString($"D{len}") : counter.CurrentValue.ToString();
        });
        return result;
    }
}
