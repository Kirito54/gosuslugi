using AutoMapper;
using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace GovServices.Server.Services;

public class GeoService : IGeoService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly GeometryFactory _geometryFactory;

    public GeoService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _geometryFactory = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory();
    }

    public async Task<List<GeoObjectDto>> GetAllAsync()
    {
        var objs = await _context.Set<GeoObject>().ToListAsync();
        return _mapper.Map<List<GeoObjectDto>>(objs);
    }

    public async Task<GeoObjectDto?> GetByIdAsync(int id)
    {
        var obj = await _context.Set<GeoObject>().FirstOrDefaultAsync(g => g.Id == id);
        return obj == null ? null : _mapper.Map<GeoObjectDto>(obj);
    }

    public async Task<GeoObjectDto> CreateAsync(CreateGeoObjectDto dto)
    {
        var reader = new WKTReader(_geometryFactory);
        var geom = reader.Read(dto.WktGeometry);

        var obj = new GeoObject
        {
            Name = dto.Name,
            Geometry = geom,
            Properties = dto.Properties
        };

        _context.Set<GeoObject>().Add(obj);
        await _context.SaveChangesAsync();

        return _mapper.Map<GeoObjectDto>(obj);
    }

    public async Task<List<GeoObjectDto>> GetIntersectingAsync(string wkt)
    {
        var reader = new WKTReader(_geometryFactory);
        var queryGeom = reader.Read(wkt);

        var objs = await _context.Set<GeoObject>()
            .Where(g => g.Geometry.Intersects(queryGeom))
            .ToListAsync();

        return _mapper.Map<List<GeoObjectDto>>(objs);
    }

    public async Task DeleteAsync(int id)
    {
        var obj = await _context.Set<GeoObject>().FirstOrDefaultAsync(g => g.Id == id);
        if (obj == null)
            return;

        _context.Set<GeoObject>().Remove(obj);
        await _context.SaveChangesAsync();
    }
}
