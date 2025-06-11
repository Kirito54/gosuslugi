using AutoMapper;
using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services;

public class ServiceService : IServiceService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ServiceService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ServiceDto>> GetAllAsync()
    {
        var services = await _context.Services.ToListAsync();
        return _mapper.Map<List<ServiceDto>>(services);
    }

    public async Task<ServiceDto> GetByIdAsync(int id)
    {
        var entity = await _context.Services.FindAsync(id);
        if (entity is null)
            throw new KeyNotFoundException($"Service с id {id} не найден");
        return _mapper.Map<ServiceDto>(entity);
    }

    public async Task<ServiceDto> CreateAsync(CreateServiceDto dto)
    {
        var entity = _mapper.Map<Service>(dto);
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Services.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<ServiceDto>(entity);
    }

    public async Task UpdateAsync(int id, UpdateServiceDto dto)
    {
        var entity = await _context.Services.FindAsync(id);
        if (entity is null)
            throw new KeyNotFoundException($"Service с id {id} не найден");
        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Services.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Services.FindAsync(id);
        if (entity is null)
            throw new KeyNotFoundException($"Service с id {id} не найден");
        _context.Services.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
