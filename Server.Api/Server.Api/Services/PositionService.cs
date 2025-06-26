using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Interfaces;
using GovServices.Server.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services;

public class PositionService : IPositionService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PositionService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PositionDto>> GetAllAsync()
    {
        var entities = await _context.Positions.ToListAsync();
        return _mapper.Map<List<PositionDto>>(entities);
    }

    public async Task<PositionDto> CreateAsync(CreatePositionDto dto)
    {
        var entity = _mapper.Map<Position>(dto);
        _context.Positions.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<PositionDto>(entity);
    }

    public async Task UpdateAsync(int id, UpdatePositionDto dto)
    {
        var entity = await _context.Positions.FindAsync(id);
        if (entity == null) throw new KeyNotFoundException();
        _mapper.Map(dto, entity);
        await _context.SaveChangesAsync();
    }
}
