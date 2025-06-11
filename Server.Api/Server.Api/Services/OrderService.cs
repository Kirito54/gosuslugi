using AutoMapper;
using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public OrderService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<List<OrderDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _db.Set<Order>()
            .Include(o => o.Signer)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<OrderDto>>(orders);
    }

    public async Task<OrderDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _db.Set<Order>()
            .Include(o => o.Signer)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        return order == null ? null : _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto> CreateAsync(CreateOrderDto dto, CancellationToken cancellationToken = default)
    {
        var prefix = dto.OrderType == "RDZ" ? "RDZ" : "RDI";
        var count = await _db.Set<Order>().CountAsync(o => o.OrderType == dto.OrderType, cancellationToken);
        var number = $"{prefix}-{(count + 1):D5}";

        var order = new Order
        {
            ApplicationId = dto.ApplicationId,
            OrderType = dto.OrderType,
            Number = number,
            Date = DateTime.UtcNow,
            Text = dto.Text,
            SignerUserId = dto.SignerUserId
        };

        _db.Set<Order>().Add(order);
        await _db.SaveChangesAsync(cancellationToken);

        return _mapper.Map<OrderDto>(order);
    }

    public async Task<bool> UpdateAsync(int id, UpdateOrderDto dto, CancellationToken cancellationToken = default)
    {
        var order = await _db.Set<Order>().FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        if (order == null)
            return false;

        order.Text = dto.Text;
        order.SignerUserId = dto.SignerUserId;

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _db.Set<Order>().FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        if (order == null)
            return false;

        _db.Remove(order);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
