using AutoMapper;
using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services;

public class ServiceTemplateService : IServiceTemplateService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public ServiceTemplateService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<ServiceTemplateDto>> GetAllAsync()
    {
        var entities = await _db.ServiceTemplates
            .Include(t => t.Service)
            .Include(t => t.UpdatedBy)
            .ToListAsync();
        return _mapper.Map<List<ServiceTemplateDto>>(entities);
    }

    public async Task<ServiceTemplateDto?> GetByIdAsync(int id)
    {
        var entity = await _db.ServiceTemplates
            .Include(t => t.Service)
            .Include(t => t.UpdatedBy)
            .FirstOrDefaultAsync(t => t.Id == id);
        return entity == null ? null : _mapper.Map<ServiceTemplateDto>(entity);
    }

    public async Task<ServiceTemplateDto> CreateAsync(CreateServiceTemplateDto dto, string userId)
    {
        var entity = _mapper.Map<ServiceTemplate>(dto);
        entity.UpdatedById = userId;
        entity.UpdatedAt = DateTime.UtcNow;
        _db.ServiceTemplates.Add(entity);
        await _db.SaveChangesAsync();
        return (await GetByIdAsync(entity.Id))!;
    }

    public async Task UpdateAsync(int id, UpdateServiceTemplateDto dto, string userId)
    {
        var entity = await _db.ServiceTemplates.FindAsync(id)
            ?? throw new KeyNotFoundException($"Template {id} not found");
        entity.JsonConfig = dto.JsonConfig;
        entity.IsActive = dto.IsActive;
        entity.UpdatedById = userId;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _db.ServiceTemplates.FindAsync(id);
        if (entity != null)
        {
            _db.ServiceTemplates.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
