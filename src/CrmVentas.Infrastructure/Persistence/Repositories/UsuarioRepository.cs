using CrmVentas.Application.Common.Interfaces;
using CrmVentas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrmVentas.Infrastructure.Persistence.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario) =>
        _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);

    public async Task AgregarAsync(Usuario usuario) =>
        await _context.Usuarios.AddAsync(usuario);
}
