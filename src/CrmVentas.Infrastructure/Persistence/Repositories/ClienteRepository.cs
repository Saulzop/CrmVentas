using CrmVentas.Application.Common.Interfaces;
using CrmVentas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmVentas.Infrastructure.Persistence.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _context;

    public ClienteRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Cliente?> ObtenerPorIdAsync(int id) =>
        _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);

    public Task<List<Cliente>> ObtenerTodosAsync() =>
        _context.Clientes.AsNoTracking().OrderBy(c => c.Nombre).ToListAsync();

    public async Task AgregarAsync(Cliente cliente) =>
        await _context.Clientes.AddAsync(cliente);

    public void Actualizar(Cliente cliente) => _context.Clientes.Update(cliente);

    public void Eliminar(Cliente cliente) => _context.Clientes.Remove(cliente);

    public Task<bool> ExisteAsync(int id) => _context.Clientes.AnyAsync(c => c.Id == id);
}
