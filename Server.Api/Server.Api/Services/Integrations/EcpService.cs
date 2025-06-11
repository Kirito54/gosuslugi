using GovServices.Server.Interfaces;

namespace GovServices.Server.Services.Integrations;

public class EcpService : IEcpService
{
    public async Task<byte[]> SignPdfAsync(byte[] pdfBytes, string certificateThumbprint)
    {
        // TODO интеграция с КриптоПро
        return await Task.FromResult(pdfBytes);
    }

    public async Task<bool> VerifySignatureAsync(byte[] signedPdfBytes)
    {
        // TODO проверка
        return await Task.FromResult(true);
    }
}
