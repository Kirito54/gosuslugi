using AutoMapper;
using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace GovServices.Server.Services;

public class DocumentService : IDocumentService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;
    private readonly IOcrService _ocrService;

    public DocumentService(
        ApplicationDbContext context,
        IMapper mapper,
        IWebHostEnvironment env,
        IOcrService ocrService)
    {
        _context = context;
        _mapper = mapper;
        _env = env;
        _ocrService = ocrService;
    }

    public async Task<List<DocumentDto>> GetByApplicationIdAsync(int applicationId)
    {
        var docs = await _context.Set<Document>()
            .Where(d => d.ApplicationId == applicationId)
            .Include(d => d.UploadedBy)
            .Include(d => d.Metadata)
            .ToListAsync();

        return _mapper.Map<List<DocumentDto>>(docs);
    }

    public async Task<DocumentDto?> GetByIdAsync(int id)
    {
        var doc = await _context.Set<Document>()
            .Include(d => d.UploadedBy)
            .Include(d => d.Metadata)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (doc == null)
            return null;

        return _mapper.Map<DocumentDto>(doc);
    }

    public async Task<DocumentDto> UploadAsync(int applicationId, IFormFile file)
    {
        var uploads = Path.Combine(_env.ContentRootPath, "Uploads", applicationId.ToString());
        Directory.CreateDirectory(uploads);

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var path = Path.Combine(uploads, fileName);

        using (var fs = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(fs);
        }

        var doc = new Document
        {
            ApplicationId = applicationId,
            FileName = file.FileName,
            FilePath = path,
            UploadedAt = DateTime.UtcNow,
            UploadedByUserId = string.Empty
        };

        _context.Set<Document>().Add(doc);
        await _context.SaveChangesAsync();

        var text = await _ocrService.RecognizeAsync(path);

        var metadata = new DocumentMetadata
        {
            DocumentId = doc.Id,
            MetadataJson = text
        };

        _context.Set<DocumentMetadata>().Add(metadata);
        await _context.SaveChangesAsync();

        doc.Metadata = metadata;

        return _mapper.Map<DocumentDto>(doc);
    }

    public async Task DeleteAsync(int id)
    {
        var doc = await _context.Set<Document>().FirstOrDefaultAsync(d => d.Id == id)
                   ?? throw new Exception("Document not found");

        if (File.Exists(doc.FilePath))
        {
            File.Delete(doc.FilePath);
        }

        _context.Set<Document>().Remove(doc);
        await _context.SaveChangesAsync();
    }
}
