using GovServices.Server.Data;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services;

public class ExtractRepository : IExtractRepository
{
    private readonly ApplicationDbContext _context;

    public ExtractRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ExtractRequest>> GetPendingRequestsAsync()
    {
        return await _context.Set<ExtractRequest>()
            .Where(r => r.Status == "Pending")
            .ToListAsync();
    }

    public async Task<bool> ProcessExtractAsync(ExtractRequest request, string response)
    {
        request.ResponseRaw = response;
        request.UpdatedAt = DateTime.UtcNow;
        request.Status = "Completed";
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task MarkAsCompletedAsync(int id)
    {
        var entity = await _context.Set<ExtractRequest>().FindAsync(id);
        if (entity != null)
        {
            entity.Status = "Completed";
            entity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
