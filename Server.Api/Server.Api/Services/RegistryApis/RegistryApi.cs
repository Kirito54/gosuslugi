using GovServices.Server.Interfaces;

namespace GovServices.Server.Services.RegistryApis;

public class RegistryApi : IRegistryApi
{
    private static readonly Dictionary<string, string> Statuses = new();

    public Task<string?> CheckStatusAsync(string externalId)
    {
        return Task.FromResult(Statuses.TryGetValue(externalId, out var status) ? status : "Pending");
    }

    public static void SetStatus(string id, string status)
    {
        Statuses[id] = status;
    }
}
