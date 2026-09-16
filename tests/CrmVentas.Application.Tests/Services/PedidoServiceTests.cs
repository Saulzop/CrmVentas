using CrmVentas.Application.Common.Exceptions;
using CrmVentas.Application.Common.Interfaces;
using CrmVentas.Application.Dtos;
using CrmVentas.Application.Services;
using CrmVentas.Domain.Modelos;
using CrmVentas.Domain.Enums;
using Moq;
using Xunit;

namespace CrmVentas.Application.Tests.Services;

public class PedidoServiceTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepository = new();
    private readonly Mock<IClienteRepository> _clienteRepository = new();
    private readonly Mock<IProductoRepository> _productoRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly PedidoService _sut;

    public PedidoServiceTests()
    {
        _sut = new PedidoService(
            _pedidoRepository.Object,
            _clienteRepository.Object,
            _productoRepository.Object,
            _unitOfWork.Object);
    }

    [Fact]
    public async Task CrearAsync_SinItems_LanzaBusinessRuleException()
    {
        var dto = new PedidoCreateDto(1, []);

        await Assert.ThrowsAsync<BusinessRuleException>(() => _sut.CrearAsync(dto));
    }

    [Fact]
    public async Task CrearAsync_ClienteNoExiste_LanzaNotFoundException()
    {
        _clienteRepository.Setup(r => r.ExisteAsync(1)).ReturnsAsync(false);
        var dto = new PedidoCreateDto(1, [new PedidoItemCreateDto(1, 2)]);

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.CrearAsync(dto));
    }

    [Fact]
    public async Task CrearAsync_StockInsuficiente_LanzaBusinessRuleException()
    {
        _clienteRepository.Setup(r => r.ExisteAsync(1)).ReturnsAsync(true);
        _productoRepository.Setup(r => r.ObtenerPorIdAsync(1))
            .ReturnsAsync(new Producto { Id = 1, Sku = "SKU-1", Nombre = "Producto 1", Precio = 100, Stock = 3 });

        var dto = new PedidoCreateDto(1, [new PedidoItemCreateDto(1, 5)]);

        var ex = await Assert.ThrowsAsync<BusinessRuleException>(() => _sut.CrearAsync(dto));
        Assert.Contains("Stock insuficiente", ex.Message);
    }

    [Fact]
    public async Task CrearAsync_ConStockSuficiente_DescuentaStockYCreaPedido()
    {
        var cliente = new Cliente { Id = 1, Nombre = "Cliente Test" };
        var producto = new Producto { Id = 1, Sku = "SKU-1", Nombre = "Producto 1", Precio = 100, Stock = 10 };

        _clienteRepository.Setup(r => r.ExisteAsync(1)).ReturnsAsync(true);
        _productoRepository.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(producto);

        Pedido? pedidoCreado = null;
        _pedidoRepository.Setup(r => r.AgregarAsync(It.IsAny<Pedido>()))
            .Callback<Pedido>(p =>
            {
                p.Cliente = cliente;
                pedidoCreado = p;
            })
            .Returns(Task.CompletedTask);
        _pedidoRepository.Setup(r => r.ObtenerPorIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => pedidoCreado);

        var dto = new PedidoCreateDto(1, [new PedidoItemCreateDto(1, 4)]);

        var resultado = await _sut.CrearAsync(dto);

        Assert.Equal(6, producto.Stock);
        Assert.Equal(400, resultado.Total);
        _productoRepository.Verify(r => r.Actualizar(producto), Times.Once);
        _unitOfWork.Verify(u => u.GuardarCambiosAsync(), Times.Once);
    }

    [Theory]
    [InlineData(EstadoPedido.Pendiente, EstadoPedido.Confirmado, true)]
    [InlineData(EstadoPedido.Pendiente, EstadoPedido.Enviado, false)]
    [InlineData(EstadoPedido.Confirmado, EstadoPedido.Enviado, true)]
    [InlineData(EstadoPedido.Enviado, EstadoPedido.Entregado, true)]
    [InlineData(EstadoPedido.Entregado, EstadoPedido.Pendiente, false)]
    [InlineData(EstadoPedido.Cancelado, EstadoPedido.Confirmado, false)]
    public async Task ActualizarEstadoAsync_ValidaTransicionesDeEstado(
        EstadoPedido estadoActual, EstadoPedido nuevoEstado, bool esValida)
    {
        var pedido = new Pedido { Id = 1, ClienteId = 1, Cliente = new Cliente { Id = 1, Nombre = "Cliente" }, Estado = estadoActual };
        _pedidoRepository.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(pedido);

        if (esValida)
        {
            var resultado = await _sut.ActualizarEstadoAsync(1, nuevoEstado);
            Assert.Equal(nuevoEstado, resultado.Estado);
            _unitOfWork.Verify(u => u.GuardarCambiosAsync(), Times.Once);
        }
        else
        {
            await Assert.ThrowsAsync<BusinessRuleException>(() => _sut.ActualizarEstadoAsync(1, nuevoEstado));
        }
    }
}
