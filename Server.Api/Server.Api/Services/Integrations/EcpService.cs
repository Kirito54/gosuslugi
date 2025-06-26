using GovServices.Server.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace GovServices.Server.Services.Integrations;

public class EcpService : IEcpService
{
    private static readonly byte[] Separator = Encoding.UTF8.GetBytes("--SIGNATURE:");

    public Task<byte[]> SignPdfAsync(byte[] pdfBytes, string certificateThumbprint)
    {
        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(pdfBytes);
        var signature = Convert.ToHexString(hash) + ":" + certificateThumbprint;
        var sigBytes = Encoding.UTF8.GetBytes(signature);
        var result = new byte[pdfBytes.Length + Separator.Length + sigBytes.Length];
        Buffer.BlockCopy(pdfBytes, 0, result, 0, pdfBytes.Length);
        Buffer.BlockCopy(Separator, 0, result, pdfBytes.Length, Separator.Length);
        Buffer.BlockCopy(sigBytes, 0, result, pdfBytes.Length + Separator.Length, sigBytes.Length);
        return Task.FromResult(result);
    }

    public Task<bool> VerifySignatureAsync(byte[] signedPdfBytes)
    {
        var idx = SearchSeparator(signedPdfBytes);
        if (idx < 0)
            return Task.FromResult(false);

        var pdf = signedPdfBytes[..idx];
        var sigStr = Encoding.UTF8.GetString(signedPdfBytes, idx + Separator.Length, signedPdfBytes.Length - idx - Separator.Length);
        var parts = sigStr.Split(':');
        if (parts.Length != 2)
            return Task.FromResult(false);

        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(pdf);
        return Task.FromResult(Convert.ToHexString(hash) == parts[0]);
    }

    private static int SearchSeparator(byte[] data)
    {
        for (int i = data.Length - Separator.Length; i >= 0; i--)
        {
            bool match = true;
            for (int j = 0; j < Separator.Length; j++)
            {
                if (data[i + j] != Separator[j]) { match = false; break; }
            }
            if (match) return i;
        }
        return -1;
    }
}
