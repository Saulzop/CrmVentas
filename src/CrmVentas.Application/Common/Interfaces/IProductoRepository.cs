using CrmVentas.Domain.Modelos;

namespace CrmVentas.Application.Common.Interfaces;

public interface IProductoRepository
{
    Task<Producto?> ObtenerPorIdAsync(int id);
    Task<List<Producto>> ObtenerTodosAsync();
    Task AgregarAsync(Producto producto);
    void Actualizar(Producto producto);
    void Eliminar(Producto producto);
    Task<bool> ExisteAsync(int id);
    Task<bool> ExisteSkuAsync(string sku);
}
