using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using GovServices.Server.Models;
using Microsoft.EntityFrameworkCore;
using RazorLight;

namespace GovServices.Server.Services;

public class TemplateService : ITemplateService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;
    private readonly IConverter _pdfConverter;
    private readonly RazorLightEngine _razorEngine;

    public TemplateService(ApplicationDbContext db, IMapper mapper, IConverter pdfConverter)
    {
        _db = db;
        _mapper = mapper;
        _pdfConverter = pdfConverter;
        _razorEngine = new RazorLightEngineBuilder()
            .UseMemoryCachingProvider()
            .Build();
    }

    public async Task<List<TemplateDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var templates = await _db.Set<Template>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return _mapper.Map<List<TemplateDto>>(templates);
    }

    public async Task<TemplateDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var template = await _db.Set<Template>()
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        return template == null ? null : _mapper.Map<TemplateDto>(template);
    }

    public async Task<TemplateDto> CreateAsync(CreateTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new Template
        {
            Name = dto.Name,
            Type = dto.Type,
            Content = dto.Content
        };

        _db.Set<Template>().Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return _mapper.Map<TemplateDto>(entity);
    }

    public async Task<bool> UpdateAsync(int id, CreateTemplateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Set<Template>().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (entity == null)
            return false;

        entity.Name = dto.Name;
        entity.Type = dto.Type;
        entity.Content = dto.Content;
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Set<Template>().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (entity == null)
            return false;

        _db.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<byte[]> GeneratePdfAsync(int templateId, TemplateModel model, CancellationToken cancellationToken = default)
    {
        var template = await _db.Set<Template>()
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == templateId, cancellationToken);
        if (template == null)
            throw new InvalidOperationException($"Template with id {templateId} not found");

        var html = await _razorEngine.CompileRenderStringAsync($"tmpl_{templateId}", template.Content, model);

        var doc = new HtmlToPdfDocument
        {
            GlobalSettings = new GlobalSettings
            {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait,
                ColorMode = ColorMode.Color
            },
            Objects =
            {
                new ObjectSettings
                {
                    HtmlContent = html,
                    WebSettings = { DefaultEncoding = "utf-8" }
                }
            }
        };

        var bytes = _pdfConverter.Convert(doc);
        return bytes;
    }
}
