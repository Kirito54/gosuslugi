using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentStorageService _storage;
    private readonly ApplicationDbContext _db;

    public DocumentsController(IDocumentStorageService storage, ApplicationDbContext db)
    {
        _storage = storage;
        _db = db;
    }

    [HttpPost("upload")]
    [Authorize]
    public async Task<ActionResult<Guid>> Upload([FromForm] DocumentUploadDto dto)
    {
        var id = await _storage.SaveAsync(dto);
        return Ok(id);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DocumentDto>> Get(Guid id)
    {
        var doc = await _db.Documents.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        if (doc == null) return NotFound();
        var dto = new DocumentDto
        {
            Id = doc.Id,
            Type = doc.Type,
            OwnerId = doc.OwnerId,
            FileName = doc.FileName,
            OriginalName = doc.OriginalName,
            MimeType = doc.MimeType,
            StoragePath = doc.StoragePath,
            Hash = doc.Hash,
            CreatedByUserId = doc.CreatedByUserId,
            CreatedAt = doc.CreatedAt,
            Visibility = doc.Visibility,
            DocumentStatus = doc.DocumentStatus,
            LinkedSEDId = doc.LinkedSEDId
        };
        return Ok(dto);
    }

    [HttpGet("owner/{ownerId}")]
    public async Task<ActionResult<List<DocumentDto>>> GetByOwner(Guid ownerId)
    {
        var docs = await _db.Documents.AsNoTracking()
            .Where(d => d.OwnerId == ownerId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();

        var list = docs.Select(doc => new DocumentDto
        {
            Id = doc.Id,
            Type = doc.Type,
            OwnerId = doc.OwnerId,
            FileName = doc.FileName,
            OriginalName = doc.OriginalName,
            MimeType = doc.MimeType,
            StoragePath = doc.StoragePath,
            Hash = doc.Hash,
            CreatedByUserId = doc.CreatedByUserId,
            CreatedAt = doc.CreatedAt,
            Visibility = doc.Visibility,
            DocumentStatus = doc.DocumentStatus,
            LinkedSEDId = doc.LinkedSEDId
        }).ToList();

        return Ok(list);
    }

    [HttpGet("download/{id}")]
    public async Task<IActionResult> Download(Guid id)
    {
        var doc = await _db.Documents.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        if (doc == null) return NotFound();
        var stream = await _storage.GetFileStreamAsync(id);
        return File(stream, doc.MimeType, doc.OriginalName);
    }

    [HttpGet("base64/{id}")]
    public async Task<ActionResult<string>> GetBase64(Guid id)
    {
        var stream = await _storage.GetFileStreamAsync(id);
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        var bytes = ms.ToArray();
        return Convert.ToBase64String(bytes);
    }

    [HttpPost("signature")]
    [Authorize]
    public async Task<IActionResult> UploadSignature(DocumentSignatureDto dto)
    {
        await _storage.SaveSignatureAsync(dto.DocumentId, dto.SignatureBase64);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _storage.DeleteAsync(id);
        if (!success) return BadRequest();
        return NoContent();
    }
}
