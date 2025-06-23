using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Interfaces;
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
}
