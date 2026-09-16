using CrmVentas.Application.Common.Exceptions;
using CrmVentas.Application.Common.Interfaces;
using CrmVentas.Application.Dtos;
using CrmVentas.Application.Services;
using CrmVentas.Domain.Modelos;
using Moq;
using Xunit;

namespace CrmVentas.Application.Tests.Services;

public class ClienteServiceTests
{
    private readonly Mock<IClienteRepository> _clienteRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly ClienteService _sut;

    public ClienteServiceTests()
    {
        _sut = new ClienteService(_clienteRepository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_ClienteNoExiste_LanzaNotFoundException()
    {
        _clienteRepository.Setup(r => r.ObtenerPorIdAsync(99)).ReturnsAsync((Cliente?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.ObtenerPorIdAsync(99));
    }

    [Fact]
    public async Task CrearAsync_GuardaClienteYRetornaDto()
    {
        var dto = new ClienteCreateDto("Nuevo Cliente", "cliente@test.com", "555-0000", "Dirección 1");

        var resultado = await _sut.CrearAsync(dto);

        Assert.Equal(dto.Nombre, resultado.Nombre);
        _clienteRepository.Verify(r => r.AgregarAsync(It.Is<Cliente>(c => c.Nombre == dto.Nombre)), Times.Once);
        _unitOfWork.Verify(u => u.GuardarCambiosAsync(), Times.Once);
    }

    [Fact]
    public async Task EliminarAsync_ClienteNoExiste_LanzaNotFoundException()
    {
        _clienteRepository.Setup(r => r.ObtenerPorIdAsync(5)).ReturnsAsync((Cliente?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.EliminarAsync(5));
    }
}
