using GovServices.Server.Interfaces;

namespace GovServices.Server.Services;

public class OcrService : IOcrService
{
    public async Task<string> RecognizeAsync(string filePath)
    {
        return await Task.Run(() =>
        {
            using var engine = new Tesseract.TesseractEngine("./tessdata", "rus", Tesseract.EngineMode.Default);
            using var img = Tesseract.Pix.LoadFromFile(filePath);
            using var page = engine.Process(img);
            return page.GetText();
        });
    }
}
