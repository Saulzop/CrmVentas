using CrmVentas.Application.Common.Interfaces;
using CrmVentas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmVentas.Infrastructure.Persistence.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly AppDbContext _context;

    public PedidoRepository(AppDbContext context)
    {
        _context = context;
    }

    private IQueryable<Pedido> ConIncludes() => _context.Pedidos
        .Include(p => p.Cliente)
        .Include(p => p.Items)
        .ThenInclude(i => i.Producto);

    public Task<Pedido?> ObtenerPorIdAsync(int id) =>
        ConIncludes().FirstOrDefaultAsync(p => p.Id == id);

    public Task<List<Pedido>> ObtenerTodosAsync() =>
        ConIncludes().AsNoTracking().OrderByDescending(p => p.FechaPedido).ToListAsync();

    public Task<List<Pedido>> ObtenerPorClienteAsync(int clienteId) =>
        ConIncludes().AsNoTracking()
            .Where(p => p.ClienteId == clienteId)
            .OrderByDescending(p => p.FechaPedido)
            .ToListAsync();

    public async Task AgregarAsync(Pedido pedido) =>
        await _context.Pedidos.AddAsync(pedido);

    public void Actualizar(Pedido pedido) => _context.Pedidos.Update(pedido);
}
