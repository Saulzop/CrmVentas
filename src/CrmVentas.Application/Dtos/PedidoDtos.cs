using CrmVentas.Domain.Enums;

namespace CrmVentas.Application.Dtos;

public record PedidoItemDto(
    int Id,
    int ProductoId,
    string ProductoNombre,
    int Cantidad,
    decimal PrecioUnitario);

public record PedidoDto(
    int Id,
    int ClienteId,
    string ClienteNombre,
    DateTime FechaPedido,
    EstadoPedido Estado,
    decimal Total,
    List<PedidoItemDto> Items);

public record PedidoItemCreateDto(int ProductoId, int Cantidad);

public record PedidoCreateDto(int ClienteId, List<PedidoItemCreateDto> Items);

public record ActualizarEstadoPedidoDto(EstadoPedido Estado);
