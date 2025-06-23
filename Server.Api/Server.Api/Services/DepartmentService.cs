using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Interfaces;
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
}
