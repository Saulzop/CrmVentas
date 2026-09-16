using CrmVentas.Application.Common.Interfaces;
using CrmVentas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmVentas.Infrastructure.Persistence.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly AppDbContext _context;

    public ProductoRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Producto?> ObtenerPorIdAsync(int id) =>
        _context.Productos.FirstOrDefaultAsync(p => p.Id == id);

    public Task<List<Producto>> ObtenerTodosAsync() =>
        _context.Productos.AsNoTracking().OrderBy(p => p.Nombre).ToListAsync();

    public async Task AgregarAsync(Producto producto) =>
        await _context.Productos.AddAsync(producto);

    public void Actualizar(Producto producto) => _context.Productos.Update(producto);

    public void Eliminar(Producto producto) => _context.Productos.Remove(producto);

    public Task<bool> ExisteAsync(int id) => _context.Productos.AnyAsync(p => p.Id == id);

    public Task<bool> ExisteSkuAsync(string sku) => _context.Productos.AnyAsync(p => p.Sku == sku);
}
