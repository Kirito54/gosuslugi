using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services;

public class SubjectService : ISubjectService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SubjectService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<IndividualDto>> GetIndividualsAsync()
    {
        var entities = await _context.Individuals.ToListAsync();
        return _mapper.Map<List<IndividualDto>>(entities);
    }

    public async Task<List<LegalEntityDto>> GetLegalEntitiesAsync()
    {
        var entities = await _context.LegalEntities.ToListAsync();
        return _mapper.Map<List<LegalEntityDto>>(entities);
    }
}
