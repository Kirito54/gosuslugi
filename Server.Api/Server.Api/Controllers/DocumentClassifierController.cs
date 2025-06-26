using GovServices.Server.DTOs;
using GovServices.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/classify")]
public class DocumentClassifierController : ControllerBase
{
    private readonly IDocumentClassifierService _classifier;

    public DocumentClassifierController(IDocumentClassifierService classifier)
    {
        _classifier = classifier;
    }

    [HttpPost]
    public async Task<ActionResult<DocumentClassificationResultDto>> Classify([FromForm] DocumentClassifyForm form)
    {
        var result = await _classifier.ClassifyAsync(form.Text, form.File);
        return Ok(result);
    }
}
