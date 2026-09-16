using CrmVentas.Application.Common.Exceptions;
using CrmVentas.Application.Common.Interfaces;
using CrmVentas.Application.Dtos;
using CrmVentas.Application.Services;
using Moq;
using Xunit;

namespace CrmVentas.Application.Tests.Services;

public class ProductoServiceTests
{
    private readonly Mock<IProductoRepository> _productoRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly ProductoService _sut;

    public ProductoServiceTests()
    {
        _sut = new ProductoService(_productoRepository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task CrearAsync_SkuDuplicado_LanzaBusinessRuleException()
    {
        _productoRepository.Setup(r => r.ExisteSkuAsync("SKU-1")).ReturnsAsync(true);
        var dto = new ProductoCreateDto("SKU-1", "Producto", null, 100, 10);

        await Assert.ThrowsAsync<BusinessRuleException>(() => _sut.CrearAsync(dto));
        _productoRepository.Verify(r => r.AgregarAsync(It.IsAny<Domain.Entities.Producto>()), Times.Never);
    }

    [Fact]
    public async Task CrearAsync_SkuNuevo_CreaProductoCorrectamente()
    {
        _productoRepository.Setup(r => r.ExisteSkuAsync("SKU-2")).ReturnsAsync(false);
        var dto = new ProductoCreateDto("SKU-2", "Producto Nuevo", "Descripción", 250.50m, 20);

        var resultado = await _sut.CrearAsync(dto);

        Assert.Equal("SKU-2", resultado.Sku);
        Assert.Equal(20, resultado.Stock);
        _unitOfWork.Verify(u => u.GuardarCambiosAsync(), Times.Once);
    }
}
