namespace GovServices.Server.Interfaces;

public interface INumberGenerator
{
    Task<string> GenerateAsync(string targetType);
}
