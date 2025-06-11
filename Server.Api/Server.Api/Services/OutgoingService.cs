using AutoMapper;
using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services;

public class OutgoingService : IOutgoingService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;

    public OutgoingService(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment env)
    {
        _context = context;
        _mapper = mapper;
        _env = env;
    }

    public async Task<List<OutgoingDocumentDto>> GetByApplicationIdAsync(int applicationId)
    {
        var docs = await _context.OutgoingDocuments
            .Include(d => d.Attachments)
            .Where(d => d.ApplicationId == applicationId)
            .ToListAsync();
        return _mapper.Map<List<OutgoingDocumentDto>>(docs);
    }

    public async Task<OutgoingDocumentDto?> GetByIdAsync(int id)
    {
        var doc = await _context.OutgoingDocuments
            .Include(d => d.Attachments)
            .FirstOrDefaultAsync(d => d.Id == id);
        return doc == null ? null : _mapper.Map<OutgoingDocumentDto>(doc);
    }

    public async Task<OutgoingDocumentDto> CreateAsync(CreateOutgoingDocumentDto dto)
    {
        var uploads = Path.Combine(_env.ContentRootPath, "OutgoingUploads", dto.ApplicationId.ToString());
        Directory.CreateDirectory(uploads);

        var fileName = Guid.NewGuid() + Path.GetExtension(dto.File.FileName);
        var filePath = Path.Combine(uploads, fileName);
        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await dto.File.CopyToAsync(stream);
        }

        var outDoc = new OutgoingDocument
        {
            ApplicationId = dto.ApplicationId,
            FileName = fileName,
            FilePath = filePath,
            Date = DateTime.UtcNow,
            Attachments = new List<OutgoingAttachment>()
        };

        _context.OutgoingDocuments.Add(outDoc);
        await _context.SaveChangesAsync();

        if (dto.Attachments != null)
        {
            foreach (var attachment in dto.Attachments)
            {
                var attachName = Guid.NewGuid() + Path.GetExtension(attachment.FileName);
                var attachPath = Path.Combine(uploads, attachName);
                await using (var stream = new FileStream(attachPath, FileMode.Create))
                {
                    await attachment.CopyToAsync(stream);
                }

                var attachEntity = new OutgoingAttachment
                {
                    OutgoingDocumentId = outDoc.Id,
                    FileName = attachName,
                    FilePath = attachPath
                };
                outDoc.Attachments.Add(attachEntity);
                _context.OutgoingAttachments.Add(attachEntity);
            }
            await _context.SaveChangesAsync();
        }

        return _mapper.Map<OutgoingDocumentDto>(outDoc);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var doc = await _context.OutgoingDocuments
            .Include(d => d.Attachments)
            .FirstOrDefaultAsync(d => d.Id == id);
        if (doc == null)
            return false;

        if (File.Exists(doc.FilePath))
            File.Delete(doc.FilePath);

        foreach (var attach in doc.Attachments)
        {
            if (File.Exists(attach.FilePath))
                File.Delete(attach.FilePath);
            _context.OutgoingAttachments.Remove(attach);
        }

        _context.OutgoingDocuments.Remove(doc);
        await _context.SaveChangesAsync();
        return true;
    }
}
