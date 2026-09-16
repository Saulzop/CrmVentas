using CrmVentas.Application.Common.Exceptions;
using CrmVentas.Application.Common.Interfaces;
using CrmVentas.Application.Dtos;
using CrmVentas.Application.Services.Interfaces;
using CrmVentas.Domain.Modelos;
using CrmVentas.Domain.Enums;

namespace CrmVentas.Application.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IUnitOfWork _unitOfWork;

    private static readonly Dictionary<EstadoPedido, EstadoPedido[]> TransicionesValidas = new()
    {
        [EstadoPedido.Pendiente] = [EstadoPedido.Confirmado, EstadoPedido.Cancelado],
        [EstadoPedido.Confirmado] = [EstadoPedido.Enviado, EstadoPedido.Cancelado],
        [EstadoPedido.Enviado] = [EstadoPedido.Entregado],
        [EstadoPedido.Entregado] = [],
        [EstadoPedido.Cancelado] = []
    };

    public PedidoService(
        IPedidoRepository pedidoRepository,
        IClienteRepository clienteRepository,
        IProductoRepository productoRepository,
        IUnitOfWork unitOfWork)
    {
        _pedidoRepository = pedidoRepository;
        _clienteRepository = clienteRepository;
        _productoRepository = productoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PedidoDto>> ObtenerTodosAsync()
    {
        var pedidos = await _pedidoRepository.ObtenerTodosAsync();
        return pedidos.Select(MapearADto).ToList();
    }

    public async Task<PedidoDto> ObtenerPorIdAsync(int id)
    {
        var pedido = await _pedidoRepository.ObtenerPorIdAsync(id)
            ?? throw new NotFoundException(nameof(Pedido), id);
        return MapearADto(pedido);
    }

    public async Task<List<PedidoDto>> ObtenerPorClienteAsync(int clienteId)
    {
        var pedidos = await _pedidoRepository.ObtenerPorClienteAsync(clienteId);
        return pedidos.Select(MapearADto).ToList();
    }

    public async Task<PedidoDto> CrearAsync(PedidoCreateDto dto)
    {
        if (dto.Items.Count == 0)
            throw new BusinessRuleException("El pedido debe tener al menos un producto.");

        if (!await _clienteRepository.ExisteAsync(dto.ClienteId))
            throw new NotFoundException(nameof(Cliente), dto.ClienteId);

        var pedido = new Pedido { ClienteId = dto.ClienteId };

        foreach (var itemDto in dto.Items)
        {
            var producto = await _productoRepository.ObtenerPorIdAsync(itemDto.ProductoId)
                ?? throw new NotFoundException(nameof(Producto), itemDto.ProductoId);

            if (itemDto.Cantidad <= 0)
                throw new BusinessRuleException($"La cantidad para '{producto.Nombre}' debe ser mayor a cero.");

            if (producto.Stock < itemDto.Cantidad)
                throw new BusinessRuleException(
                    $"Stock insuficiente para '{producto.Nombre}'. Disponible: {producto.Stock}, solicitado: {itemDto.Cantidad}.");

            producto.Stock -= itemDto.Cantidad;
            _productoRepository.Actualizar(producto);

            pedido.Items.Add(new PedidoItem
            {
                ProductoId = producto.Id,
                Producto = producto,
                Cantidad = itemDto.Cantidad,
                PrecioUnitario = producto.Precio
            });
        }

        await _pedidoRepository.AgregarAsync(pedido);
        await _unitOfWork.GuardarCambiosAsync();

        var pedidoCreado = await _pedidoRepository.ObtenerPorIdAsync(pedido.Id)
            ?? throw new NotFoundException(nameof(Pedido), pedido.Id);
        return MapearADto(pedidoCreado);
    }

    public async Task<PedidoDto> ActualizarEstadoAsync(int id, EstadoPedido nuevoEstado)
    {
        var pedido = await _pedidoRepository.ObtenerPorIdAsync(id)
            ?? throw new NotFoundException(nameof(Pedido), id);

        if (pedido.Estado == nuevoEstado)
            return MapearADto(pedido);

        var estadosPermitidos = TransicionesValidas[pedido.Estado];
        if (!estadosPermitidos.Contains(nuevoEstado))
            throw new BusinessRuleException(
                $"No se puede cambiar el pedido de '{pedido.Estado}' a '{nuevoEstado}'.");

        pedido.Estado = nuevoEstado;
        _pedidoRepository.Actualizar(pedido);
        await _unitOfWork.GuardarCambiosAsync();

        return MapearADto(pedido);
    }

    private static PedidoDto MapearADto(Pedido pedido) => new(
        pedido.Id,
        pedido.ClienteId,
        pedido.Cliente?.Nombre ?? string.Empty,
        pedido.FechaPedido,
        pedido.Estado,
        pedido.Total,
        pedido.Items.Select(i => new PedidoItemDto(
            i.Id,
            i.ProductoId,
            i.Producto?.Nombre ?? string.Empty,
            i.Cantidad,
            i.PrecioUnitario)).ToList());
}
