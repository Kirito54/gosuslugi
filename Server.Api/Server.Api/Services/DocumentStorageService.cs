using System.Security.Claims;
using System.Security.Cryptography;
using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services;

public class DocumentStorageService : IDocumentStorageService
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _http;

    public DocumentStorageService(ApplicationDbContext db, IWebHostEnvironment env, IHttpContextAccessor http)
    {
        _db = db;
        _env = env;
        _http = http;
    }

    public async Task<Guid> SaveAsync(DocumentUploadDto dto)
    {
        var userId = _http.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? throw new UnauthorizedAccessException();

        var now = DateTime.UtcNow;
        var folder = Path.Combine(_env.ContentRootPath, "Documents",
            now.ToString("yyyy"), now.ToString("MM"), now.ToString("dd"));
        Directory.CreateDirectory(folder);

        var id = Guid.NewGuid();
        var extension = Path.GetExtension(dto.File.FileName);
        var fileName = id + extension;
        var path = Path.Combine(folder, fileName);

        using (var fs = new FileStream(path, FileMode.Create))
        {
            await dto.File.CopyToAsync(fs);
        }

        string hash;
        using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        using (var sha = SHA256.Create())
        {
            var bytes = await sha.ComputeHashAsync(fs);
            hash = Convert.ToHexString(bytes);
        }

        // archive previous versions
        var existing = await _db.Documents
            .Where(d => d.OwnerId == dto.OwnerId && d.Type == dto.Type && d.DocumentStatus != DocumentStatusType.Archived)
            .ToListAsync();
        foreach (var doc in existing)
        {
            doc.DocumentStatus = DocumentStatusType.Archived;
        }

        var entity = new Document
        {
            Id = id,
            Type = dto.Type,
            OwnerId = dto.OwnerId,
            FileName = fileName,
            OriginalName = dto.File.FileName,
            MimeType = dto.File.ContentType,
            StoragePath = path,
            Hash = hash,
            CreatedByUserId = userId,
            CreatedAt = now,
            Visibility = dto.Visibility,
            DocumentStatus = DocumentStatusType.Draft
        };

        _db.Documents.Add(entity);
        await _db.SaveChangesAsync();
        return id;
    }

    public async Task<Stream> GetFileStreamAsync(Guid id)
    {
        var doc = await _db.Documents.FirstOrDefaultAsync(d => d.Id == id)
                  ?? throw new FileNotFoundException();
        return new FileStream(doc.StoragePath, FileMode.Open, FileAccess.Read, FileShare.Read);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var doc = await _db.Documents.FirstOrDefaultAsync(d => d.Id == id);
        if (doc == null || doc.DocumentStatus != DocumentStatusType.Draft)
            return false;

        if (File.Exists(doc.StoragePath))
            File.Delete(doc.StoragePath);

        _db.Documents.Remove(doc);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task SaveSignatureAsync(Guid documentId, string base64)
    {
        var doc = await _db.Documents.FirstOrDefaultAsync(d => d.Id == documentId)
                  ?? throw new FileNotFoundException();

        var sigPath = Path.ChangeExtension(doc.StoragePath, ".sig");
        await File.WriteAllTextAsync(sigPath, base64);
    }
}
