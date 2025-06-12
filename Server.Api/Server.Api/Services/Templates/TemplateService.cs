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

namespace GovServices.Server.Services.Templates;

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
            .UseEmbeddedResourcesProject(typeof(TemplateService))
            .UseMemoryCachingProvider()
            .Build();
    }

    public async Task<List<TemplateDto>> GetAllAsync()
    {
        var list = await _db.Set<Template>().AsNoTracking().ToListAsync();
        return _mapper.Map<List<TemplateDto>>(list);
    }

    public async Task<TemplateDto?> GetByIdAsync(int id)
    {
        var template = await _db.Set<Template>().AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        return template == null ? null : _mapper.Map<TemplateDto>(template);
    }

    public async Task<TemplateDto> CreateAsync(CreateTemplateDto dto)
    {
        var entity = _mapper.Map<Template>(dto);
        _db.Set<Template>().Add(entity);
        await _db.SaveChangesAsync();
        return _mapper.Map<TemplateDto>(entity);
    }

    public async Task UpdateAsync(int id, UpdateTemplateDto dto)
    {
        var entity = await _db.Set<Template>().FirstOrDefaultAsync(t => t.Id == id)
                     ?? throw new KeyNotFoundException($"Template {id} not found");

        entity.Name = dto.Name;
        entity.Type = dto.Type;
        entity.Content = dto.Content;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _db.Set<Template>().FirstOrDefaultAsync(t => t.Id == id)
                     ?? throw new KeyNotFoundException($"Template {id} not found");
        _db.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<byte[]> GeneratePdfAsync(int templateId, TemplateModel model)
    {
        var template = await _db.Set<Template>().AsNoTracking().FirstOrDefaultAsync(t => t.Id == templateId)
                       ?? throw new KeyNotFoundException($"Template {templateId} not found");

        var html = await _razorEngine.CompileRenderStringAsync($"tmpl_{templateId}", template.Content, model);

        var doc = new HtmlToPdfDocument
        {
            GlobalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10, Bottom = 10 }
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
