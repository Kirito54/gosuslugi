namespace GovServices.Server.Interfaces;

using Microsoft.AspNetCore.Http;
using GovServices.Server.DTOs;

public interface IDocumentClassifierService
{
    Task<DocumentClassificationResultDto> ClassifyAsync(string? text, IFormFile? file);
}
