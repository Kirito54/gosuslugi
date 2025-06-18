using AutoMapper;
using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services.Numbering;

public class NumberTemplateService : INumberTemplateService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public NumberTemplateService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<NumberTemplateDto>> GetAllAsync()
    {
        var list = await _db.NumberTemplates.AsNoTracking().ToListAsync();
        return _mapper.Map<List<NumberTemplateDto>>(list);
    }

    public async Task<NumberTemplateDto?> GetByIdAsync(int id)
    {
        var entity = await _db.NumberTemplates.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        return entity == null ? null : _mapper.Map<NumberTemplateDto>(entity);
    }

    public async Task<NumberTemplateDto> CreateAsync(CreateNumberTemplateDto dto)
    {
        var entity = _mapper.Map<NumberTemplate>(dto);
        _db.NumberTemplates.Add(entity);
        await _db.SaveChangesAsync();
        return _mapper.Map<NumberTemplateDto>(entity);
    }

    public async Task UpdateAsync(int id, UpdateNumberTemplateDto dto)
    {
        var entity = await _db.NumberTemplates.FirstOrDefaultAsync(t => t.Id == id) ?? throw new KeyNotFoundException();
        entity.Name = dto.Name;
        entity.TargetType = dto.TargetType;
        entity.TemplateText = dto.TemplateText;
        entity.ResetPolicy = dto.ResetPolicy;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _db.NumberTemplates.FirstOrDefaultAsync(t => t.Id == id);
        if (entity == null) return;
        _db.NumberTemplates.Remove(entity);
        await _db.SaveChangesAsync();
    }
}
