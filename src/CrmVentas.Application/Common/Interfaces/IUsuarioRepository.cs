using CrmVentas.Domain.Modelos;

namespace CrmVentas.Application.Common.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario);
    Task AgregarAsync(Usuario usuario);
}
