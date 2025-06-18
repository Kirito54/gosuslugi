using GovServices.Server.Entities;

namespace GovServices.Server.Interfaces;

public interface IExtractRepository
{
    Task<List<ExtractRequest>> GetPendingRequestsAsync();
    Task<bool> ProcessExtractAsync(ExtractRequest request, string response);
    Task MarkAsCompletedAsync(int id);
}
