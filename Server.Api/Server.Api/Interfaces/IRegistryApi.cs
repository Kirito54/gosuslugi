namespace GovServices.Server.Interfaces;

public interface IRegistryApi
{
    Task<string?> CheckStatusAsync(string externalId);
}
