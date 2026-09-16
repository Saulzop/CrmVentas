using CrmVentas.Domain.Entities;

namespace CrmVentas.Application.Common.Interfaces;

public interface IClienteRepository
{
    Task<Cliente?> ObtenerPorIdAsync(int id);
    Task<List<Cliente>> ObtenerTodosAsync();
    Task AgregarAsync(Cliente cliente);
    void Actualizar(Cliente cliente);
    void Eliminar(Cliente cliente);
    Task<bool> ExisteAsync(int id);
}
