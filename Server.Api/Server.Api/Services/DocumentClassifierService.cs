using System.Text;
using System.Text.RegularExpressions;
using GovServices.Server.DTOs;
using GovServices.Server.Interfaces;
using Microsoft.AspNetCore.Http;
using UglyToad.PdfPig;

namespace GovServices.Server.Services;

public class DocumentClassifierService : IDocumentClassifierService
{
    private readonly IOcrService _ocr;

    public DocumentClassifierService(IOcrService ocr)
    {
        _ocr = ocr;
    }

    public async Task<DocumentClassificationResultDto> ClassifyAsync(string? text, IFormFile? file)
    {
        var content = await GetTextAsync(text, file);
        var (type, fields) = ClassifyText(content);
        return new DocumentClassificationResultDto { Type = type, Fields = fields };
    }

    private async Task<string> GetTextAsync(string? text, IFormFile? file)
    {
        if (file != null)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (ext == ".pdf")
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                ms.Position = 0;
                using var pdf = PdfDocument.Open(ms);
                var sb = new StringBuilder();
                foreach (var page in pdf.GetPages())
                {
                    sb.AppendLine(page.Text);
                }
                return sb.ToString();
            }
            else if (ext == ".jpg" || ext == ".jpeg" || ext == ".png")
            {
                var temp = Path.GetTempFileName() + ext;
                using (var fs = new FileStream(temp, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }
                var result = await _ocr.RecognizeAsync(temp);
                File.Delete(temp);
                return result;
            }
            else
            {
                using var reader = new StreamReader(file.OpenReadStream());
                return await reader.ReadToEndAsync();
            }
        }

        if (!string.IsNullOrWhiteSpace(text))
        {
            return text;
        }

        throw new ArgumentException("text or file required");
    }

    private static (string, Dictionary<string, string>) ClassifyText(string text)
    {
        var lowered = text.ToLowerInvariant();
        var fields = new Dictionary<string, string>();
        string type = "unknown";

        if (lowered.Contains("паспорт"))
        {
            type = "passport";
            var series = Regex.Match(lowered, @"серия\s*(\d{2}\s*\d{2})");
            var number = Regex.Match(lowered, @"номер\s*(\d{6})");
            var issued = Regex.Match(lowered, @"выдан\s*([^,\n]+)");
            var date = Regex.Match(lowered, @"(\d{2}\.\d{2}\.\d{4})");
            if (series.Success) fields["series"] = series.Groups[1].Value;
            if (number.Success) fields["number"] = number.Groups[1].Value;
            if (issued.Success) fields["issued_by"] = issued.Groups[1].Value.Trim();
            if (date.Success) fields["issue_date"] = date.Groups[1].Value;
        }
        else if (lowered.Contains("заявление"))
        {
            type = "statement";
            var name = Regex.Match(text, @"от\s*([А-Яа-яЁё\s]+)");
            var date = Regex.Match(text, @"(\d{2}\.\d{2}\.\d{4})");
            if (name.Success) fields["applicant"] = name.Groups[1].Value.Trim();
            if (date.Success) fields["date"] = date.Groups[1].Value;
        }
        else if (lowered.Contains("доверенн"))
        {
            type = "power_of_attorney";
            var principal = Regex.Match(text, @"доверитель[:\s]*([А-Яа-яЁё\s]+)", RegexOptions.IgnoreCase);
            var agent = Regex.Match(text, @"представитель[:\s]*([А-Яа-яЁё\s]+)", RegexOptions.IgnoreCase);
            var date = Regex.Match(text, @"(\d{2}\.\d{2}\.\d{4})");
            if (principal.Success) fields["principal"] = principal.Groups[1].Value.Trim();
            if (agent.Success) fields["agent"] = agent.Groups[1].Value.Trim();
            if (date.Success) fields["date"] = date.Groups[1].Value;
        }
        else if (lowered.Contains("схема") || lowered.Contains("план"))
        {
            type = "scheme";
            var title = Regex.Match(text, @"схема[:\s]*([\w\s]+)", RegexOptions.IgnoreCase);
            if (title.Success) fields["title"] = title.Groups[1].Value.Trim();
        }

        return (type, fields);
    }
}
