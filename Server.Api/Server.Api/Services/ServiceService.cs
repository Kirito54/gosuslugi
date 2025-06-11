using AutoMapper;
using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services;

public class ServiceService : IServiceService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public ServiceService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<ServiceDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var services = await _db.Set<Service>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return _mapper.Map<List<ServiceDto>>(services);
    }

    public async Task<ServiceDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var service = await _db.Set<Service>()
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        return service == null ? null : _mapper.Map<ServiceDto>(service);
    }

    public async Task<ServiceDto> CreateAsync(ServiceDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Service>(dto);
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;

        _db.Set<Service>().Add(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ServiceDto>(entity);
    }

    public async Task<bool> UpdateAsync(ServiceDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Set<Service>().FirstOrDefaultAsync(s => s.Id == dto.Id, cancellationToken);
        if (entity == null)
            return false;

        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.Set<Service>().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        if (entity == null)
            return false;

        _db.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
