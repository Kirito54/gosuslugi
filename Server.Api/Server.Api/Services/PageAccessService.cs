using AutoMapper;
using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services;

public class PageAccessService : IPageAccessService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PageAccessService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PageAccessDto>> GetAllAsync()
    {
        var items = await _context.PageAccesses.ToListAsync();
        return _mapper.Map<List<PageAccessDto>>(items);
    }

    public async Task<PageAccessDto> CreateAsync(CreatePageAccessDto dto)
    {
        var entity = _mapper.Map<PageAccess>(dto);
        _context.PageAccesses.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<PageAccessDto>(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.PageAccesses.FindAsync(id);
        if (entity != null)
        {
            _context.PageAccesses.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
