using CrmVentas.Application.Dtos;
using CrmVentas.Domain.Enums;

namespace CrmVentas.Application.Services.Interfaces;

public interface IPedidoService
{
    Task<List<PedidoDto>> ObtenerTodosAsync();
    Task<PedidoDto> ObtenerPorIdAsync(int id);
    Task<List<PedidoDto>> ObtenerPorClienteAsync(int clienteId);
    Task<PedidoDto> CrearAsync(PedidoCreateDto dto);
    Task<PedidoDto> ActualizarEstadoAsync(int id, EstadoPedido nuevoEstado);
}
