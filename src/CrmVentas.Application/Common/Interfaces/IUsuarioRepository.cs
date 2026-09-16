using CrmVentas.Domain.Entities;

namespace CrmVentas.Application.Common.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario);
    Task AgregarAsync(Usuario usuario);
}
