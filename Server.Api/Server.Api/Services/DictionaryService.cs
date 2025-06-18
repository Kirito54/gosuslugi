using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using EPPlus.Core.Extensions;
using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System.Globalization;

namespace GovServices.Server.Services;

public class DictionaryService : IDictionaryService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DictionaryService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public async Task<List<DictionaryDto>> GetAllAsync()
    {
        var list = await _context.Dictionaries.ToListAsync();
        return list.Select(e => new DictionaryDto
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            UpdatedAt = e.UpdatedAt,
            SourceType = e.SourceType,
            RecordCount = GetRecordCount(e.Data)
        }).ToList();
    }

    public async Task<DictionaryDto?> GetByIdAsync(int id)
    {
        var entity = await _context.Dictionaries.FindAsync(id);
        if (entity == null) return null;
        return new DictionaryDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            UpdatedAt = entity.UpdatedAt,
            SourceType = entity.SourceType,
            RecordCount = GetRecordCount(entity.Data)
        };
    }

    public async Task<int> CreateAsync(UploadDictionaryDto dto)
    {
        var entity = new Dictionary
        {
            Name = dto.Name,
            Description = dto.Description,
            SourceType = dto.SourceType,
            SourceUrl = dto.SourceUrl,
            UpdatedAt = DateTime.UtcNow,
            Data = await ParseFileAsync(dto.File)
        };
        _context.Dictionaries.Add(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    private int GetRecordCount(string data)
    {
        if (string.IsNullOrWhiteSpace(data)) return 0;
        return JArray.Parse(data).Count;
    }

    private async Task<string> ParseFileAsync(IFormFile file)
    {
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        switch (ext)
        {
            case ".csv":
                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<dynamic>().ToList();
                    return Newtonsoft.Json.JsonConvert.SerializeObject(records);
                }
            case ".xlsx":
                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var ws = package.Workbook.Worksheets.First();
                    var list = new List<Dictionary<string, string>>();
                    var cols = ws.Dimension.Columns;
                    var rows = ws.Dimension.Rows;
                    var headers = new List<string>();
                    for (int c = 1; c <= cols; c++) headers.Add(ws.Cells[1, c].Text);
                    for (int r = 2; r <= rows; r++)
                    {
                        var item = new Dictionary<string, string>();
                        for (int c = 1; c <= cols; c++)
                            item[headers[c - 1]] = ws.Cells[r, c].Text;
                        list.Add(item);
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(list);
                }
            case ".json":
                using (var sr = new StreamReader(file.OpenReadStream()))
                {
                    return await sr.ReadToEndAsync();
                }
            default:
                throw new InvalidOperationException("Unsupported file type");
        }
    }
}
