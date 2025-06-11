namespace GovServices.Server.Interfaces;

public interface IEcpService
{
    Task<byte[]> SignPdfAsync(byte[] pdfBytes, string certificateThumbprint);
    Task<bool> VerifySignatureAsync(byte[] signedPdfBytes);
}
