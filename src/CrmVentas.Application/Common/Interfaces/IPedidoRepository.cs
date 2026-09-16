using CrmVentas.Domain.Entities;

namespace CrmVentas.Application.Common.Interfaces;

public interface IPedidoRepository
{
    Task<Pedido?> ObtenerPorIdAsync(int id);
    Task<List<Pedido>> ObtenerTodosAsync();
    Task<List<Pedido>> ObtenerPorClienteAsync(int clienteId);
    Task AgregarAsync(Pedido pedido);
    void Actualizar(Pedido pedido);
}
