using CrmVentas.Application.Dtos;

namespace CrmVentas.Application.Services.Interfaces;

public interface IProductoService
{
    Task<List<ProductoDto>> ObtenerTodosAsync();
    Task<ProductoDto> ObtenerPorIdAsync(int id);
    Task<ProductoDto> CrearAsync(ProductoCreateDto dto);
    Task<ProductoDto> ActualizarAsync(int id, ProductoUpdateDto dto);
    Task EliminarAsync(int id);
}
