namespace GovServices.Server.Interfaces;

public interface IOcrService
{
    Task<string> RecognizeAsync(string filePath);
}
