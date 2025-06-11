using AutoMapper;
using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

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
        _geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory();
    }

    public async Task<List<GeoObjectDto>> GetAllAsync()
    {
        var entities = await _context.GeoObjects.ToListAsync();
        return _mapper.Map<List<GeoObjectDto>>(entities);
    }

    public async Task<GeoObjectDto?> GetByIdAsync(int id)
    {
        var entity = await _context.GeoObjects.FindAsync(id);
        return entity != null ? _mapper.Map<GeoObjectDto>(entity) : null;
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

        _context.GeoObjects.Add(obj);
        await _context.SaveChangesAsync();

        return _mapper.Map<GeoObjectDto>(obj);
    }

    public async Task<List<GeoObjectDto>> GetIntersectingAsync(string wkt)
    {
        var reader = new WKTReader(_geometryFactory);
        var queryGeom = reader.Read(wkt);

        var entities = await _context.GeoObjects.ToListAsync();
        var intersecting = entities
            .Where(g => g.Geometry != null && g.Geometry.Intersects(queryGeom))
            .ToList();

        return _mapper.Map<List<GeoObjectDto>>(intersecting);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.GeoObjects.FindAsync(id);
        if (entity != null)
        {
            _context.GeoObjects.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
