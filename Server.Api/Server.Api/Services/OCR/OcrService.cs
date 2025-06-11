using GovServices.Server.Interfaces;
using Tesseract;

namespace GovServices.Server.Services;

public class OcrService : IOcrService
{
    public async Task<string> RecognizeAsync(string filePath)
    {
        return await Task.Run(() =>
        {
            using var engine = new TesseractEngine("./tessdata", "rus", EngineMode.Default);
            using var img = Pix.LoadFromFile(filePath);
            using var page = engine.Process(img);
            return page.GetText();
        });
    }
}
