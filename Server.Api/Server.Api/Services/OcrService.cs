using GovServices.Server.Interfaces;

namespace GovServices.Server.Services;

public class OcrService : IOcrService
{
    public Task<string> RecognizeAsync(string filePath)
    {
        return Task.FromResult(string.Empty);
    }
}
