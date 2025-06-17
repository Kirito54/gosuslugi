using AutoMapper;
using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services;

    public class ApplicationService : IApplicationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWorkflowService _workflowService;

    public ApplicationService(ApplicationDbContext context, IMapper mapper, IWorkflowService workflowService)
    {
        _context = context;
        _mapper = mapper;
        _workflowService = workflowService;
    }

    public async Task<List<ApplicationDto>> GetAllAsync()
    {
        var apps = await _context.Applications
            .Include(a => a.Service)
            .Include(a => a.CurrentStep)
            .Include(a => a.AssignedTo)
            .ToListAsync();
        return _mapper.Map<List<ApplicationDto>>(apps);
    }

    public async Task<ApplicationDto> GetByIdAsync(int id)
    {
        var app = await _context.Applications
            .Include(a => a.Service)
            .Include(a => a.CurrentStep)
            .Include(a => a.AssignedTo)
            .FirstOrDefaultAsync(a => a.Id == id);
        if (app == null)
            throw new KeyNotFoundException($"Application {id} not found");

        return _mapper.Map<ApplicationDto>(app);
    }

    public async Task<ApplicationDto> CreateAsync(CreateApplicationDto dto)
    {
        var todayCount = await _context.Applications.CountAsync(a => a.CreatedAt.Date == DateTime.UtcNow.Date);
        var number = $"{DateTime.UtcNow:yyyyMMdd}-{(todayCount + 1):D5}";

        var app = new Application
        {
            Number = number,
            ServiceId = dto.ServiceId,
            Status = "New",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var workflow = await _context.Workflows
            .Include(w => w.Steps)
            .FirstOrDefaultAsync(w => w.Id == dto.ServiceId);

        if (workflow != null)
        {
            var firstStep = workflow.Steps.OrderBy(s => s.Sequence).FirstOrDefault();
            if (firstStep != null)
            {
                app.CurrentStepId = firstStep.Id;
            }
        }

        _context.Applications.Add(app);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(app.Id);
    }

    public async Task UpdateAsync(int id, UpdateApplicationDto dto)
    {
        var app = await _context.Applications.FirstOrDefaultAsync(a => a.Id == id);
        if (app == null)
            throw new KeyNotFoundException($"Application {id} not found");

        app.CurrentStepId = dto.CurrentStepId;
        app.Status = dto.Status;
        app.AssignedToUserId = dto.AssignedToUserId;
        app.UpdatedAt = DateTime.UtcNow;

        _context.ApplicationLogs.Add(new ApplicationLog
        {
            ApplicationId = id,
            Action = "Updated application",
            UserId = "TODO",
            Timestamp = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var app = await _context.Applications.FirstOrDefaultAsync(a => a.Id == id);
        if (app == null)
            return;

        _context.Applications.Remove(app);
        await _context.SaveChangesAsync();
    }

    public async Task AdvanceAsync(int applicationId, object contextData)
    {
        var app = await _context.Applications
            .Include(a => a.CurrentStep)
            .FirstOrDefaultAsync(a => a.Id == applicationId);
        if (app == null)
            throw new KeyNotFoundException($"Application {applicationId} not found");

        var next = await _workflowService.GetNextStepAsync(app.CurrentStepId, contextData);
        if (next == null)
            throw new InvalidOperationException("No next step available");

        app.CurrentStepId = next.Id;
        app.UpdatedAt = DateTime.UtcNow;

        _context.ApplicationLogs.Add(new ApplicationLog
        {
            ApplicationId = applicationId,
            Action = $"Advanced to {next.Name}",
            UserId = "TODO",
            Timestamp = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
    }

    public async Task<List<ApplicationLogDto>> GetLogsAsync(int applicationId)
    {
        var logs = await _context.ApplicationLogs
            .Where(l => l.ApplicationId == applicationId)
            .Include(l => l.User)
            .OrderByDescending(l => l.Timestamp)
            .ToListAsync();
        return _mapper.Map<List<ApplicationLogDto>>(logs);
    }

    public async Task<List<ApplicationResultDto>> GetResultsAsync(int applicationId)
    {
        var results = await _context.ApplicationResults
            .Where(r => r.ApplicationId == applicationId)
            .Include(r => r.Document)
            .ToListAsync();
        return _mapper.Map<List<ApplicationResultDto>>(results);
    }

    public async Task<ApplicationResultDto> AddResultAsync(CreateApplicationResultDto dto)
    {
        var entity = _mapper.Map<ApplicationResult>(dto);
        entity.LinkedAt = DateTime.UtcNow;
        _context.ApplicationResults.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<ApplicationResultDto>(entity);
    }

    public async Task<List<ApplicationRevisionDto>> GetRevisionsAsync(int applicationId)
    {
        var revisions = await _context.ApplicationRevisions
            .Where(r => r.ApplicationId == applicationId)
            .ToListAsync();
        return _mapper.Map<List<ApplicationRevisionDto>>(revisions);
    }

    public async Task<ApplicationRevisionDto> AddRevisionAsync(CreateApplicationRevisionDto dto)
    {
        var entity = _mapper.Map<ApplicationRevision>(dto);
        entity.CreatedAt = DateTime.UtcNow;
        _context.ApplicationRevisions.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<ApplicationRevisionDto>(entity);
    }

    public async Task<List<ApplicationDto>> GetByApplicantAsync(int applicationId)
    {
        var apps = await _context.Applications
            .Where(a => a.Id != applicationId)
            .Include(a => a.Service)
            .Include(a => a.CurrentStep)
            .Include(a => a.AssignedTo)
            .Take(10)
            .ToListAsync();
        return _mapper.Map<List<ApplicationDto>>(apps);
    }

    public async Task<List<ApplicationDto>> GetByRepresentativeAsync(int applicationId)
    {
        var apps = await _context.Applications
            .Where(a => a.Id != applicationId)
            .Include(a => a.Service)
            .Include(a => a.CurrentStep)
            .Include(a => a.AssignedTo)
            .Take(10)
            .ToListAsync();
        return _mapper.Map<List<ApplicationDto>>(apps);
    }

    public async Task<List<ApplicationDto>> GetByGeoIntersectionAsync(int applicationId)
    {
        var apps = await _context.Applications
            .Where(a => a.Id != applicationId)
            .Include(a => a.Service)
            .Include(a => a.CurrentStep)
            .Include(a => a.AssignedTo)
            .Take(10)
            .ToListAsync();
        return _mapper.Map<List<ApplicationDto>>(apps);
    }
}
