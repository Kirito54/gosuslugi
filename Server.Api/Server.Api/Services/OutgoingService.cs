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

public class OutgoingService : IOutgoingService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;

    public OutgoingService(
        ApplicationDbContext context,
        IMapper mapper,
        IWebHostEnvironment env)
    {
        _context = context;
        _mapper = mapper;
        _env = env;
    }

    public async Task<List<OutgoingDocumentDto>> GetByApplicationIdAsync(int applicationId)
    {
        var docs = await _context.Set<OutgoingDocument>()
            .Include(d => d.Attachments)
            .Where(d => d.ApplicationId == applicationId)
            .ToListAsync();

        return _mapper.Map<List<OutgoingDocumentDto>>(docs);
    }

    public async Task<OutgoingDocumentDto?> GetByIdAsync(int id)
    {
        var doc = await _context.Set<OutgoingDocument>()
            .Include(d => d.Attachments)
            .FirstOrDefaultAsync(d => d.Id == id);

        return doc == null ? null : _mapper.Map<OutgoingDocumentDto>(doc);
    }

    public async Task<OutgoingDocumentDto> CreateAsync(CreateOutgoingDocumentDto dto)
    {
        var uploads = Path.Combine(_env.ContentRootPath, "OutgoingUploads", dto.ApplicationId.ToString());
        Directory.CreateDirectory(uploads);

        var mainFileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
        var mainPath = Path.Combine(uploads, mainFileName);

        using (var fs = new FileStream(mainPath, FileMode.Create))
        {
            await dto.File.CopyToAsync(fs);
        }

        var outDoc = new OutgoingDocument
        {
            ApplicationId = dto.ApplicationId,
            FileName = dto.File.FileName,
            FilePath = mainPath,
            Date = DateTime.UtcNow
        };

        _context.Set<OutgoingDocument>().Add(outDoc);
        await _context.SaveChangesAsync();

        foreach (var attachment in dto.Attachments ?? new List<IFormFile>())
        {
            var attFileName = $"{Guid.NewGuid()}_{attachment.FileName}";
            var attPath = Path.Combine(uploads, attFileName);

            using (var fs = new FileStream(attPath, FileMode.Create))
            {
                await attachment.CopyToAsync(fs);
            }

            _context.Set<OutgoingAttachment>().Add(new OutgoingAttachment
            {
                OutgoingDocumentId = outDoc.Id,
                FileName = attachment.FileName,
                FilePath = attPath
            });
        }

        await _context.SaveChangesAsync();

        outDoc = await _context.Set<OutgoingDocument>()
            .Include(d => d.Attachments)
            .FirstAsync(d => d.Id == outDoc.Id);

        return _mapper.Map<OutgoingDocumentDto>(outDoc);
    }

    public async Task DeleteAsync(int id)
    {
        var doc = await _context.Set<OutgoingDocument>()
            .Include(d => d.Attachments)
            .FirstOrDefaultAsync(d => d.Id == id)
                   ?? throw new Exception("Outgoing document not found");

        if (File.Exists(doc.FilePath))
        {
            File.Delete(doc.FilePath);
        }

        foreach (var att in doc.Attachments)
        {
            if (File.Exists(att.FilePath))
            {
                File.Delete(att.FilePath);
            }
        }

        _context.Set<OutgoingAttachment>().RemoveRange(doc.Attachments);
        _context.Set<OutgoingDocument>().Remove(doc);
        await _context.SaveChangesAsync();
    }
}
