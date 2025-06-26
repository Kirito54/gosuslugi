using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Interfaces;
using GovServices.Server.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services;

public class DepartmentService : IDepartmentService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DepartmentService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<DepartmentDto>> GetAllAsync()
    {
        var entities = await _context.Departments.ToListAsync();
        return _mapper.Map<List<DepartmentDto>>(entities);
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
    {
        var entity = _mapper.Map<Department>(dto);
        _context.Departments.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<DepartmentDto>(entity);
    }

    public async Task UpdateAsync(int id, UpdateDepartmentDto dto)
    {
        var entity = await _context.Departments.FindAsync(id);
        if (entity == null) throw new KeyNotFoundException();
        _mapper.Map(dto, entity);
        await _context.SaveChangesAsync();
    }
}
