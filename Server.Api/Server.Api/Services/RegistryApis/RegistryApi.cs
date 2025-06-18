using GovServices.Server.Interfaces;

namespace GovServices.Server.Services.RegistryApis;

public class RegistryApi : IRegistryApi
{
    public Task<string?> CheckStatusAsync(string externalId)
    {
        // TODO: implement registry interaction
        return Task.FromResult<string?>(null);
    }
}
