using GovServices.Server.Interfaces;

namespace GovServices.Server.Services.RegistryApis;

public class ZagcApi : IRegistryApi
{
    public Task<string?> CheckStatusAsync(string externalId)
    {
        // TODO: call ZAGS API
        return Task.FromResult<string?>(null);
    }
}
