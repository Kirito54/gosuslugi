using GovServices.Server.Interfaces;

namespace GovServices.Server.Services.RegistryApis;

public class RosreestrApi : IRegistryApi
{
    public Task<string?> CheckStatusAsync(string externalId)
    {
        // TODO: call Rosreestr API
        return Task.FromResult<string?>(null);
    }
}
