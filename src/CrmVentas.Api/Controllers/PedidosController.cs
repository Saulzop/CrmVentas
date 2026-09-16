using CrmVentas.Application.Dtos;
using CrmVentas.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmVentas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidosController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PedidoDto>>> ObtenerTodos() =>
        Ok(await _pedidoService.ObtenerTodosAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PedidoDto>> ObtenerPorId(int id) =>
        Ok(await _pedidoService.ObtenerPorIdAsync(id));

    [HttpGet("cliente/{clienteId:int}")]
    public async Task<ActionResult<List<PedidoDto>>> ObtenerPorCliente(int clienteId) =>
        Ok(await _pedidoService.ObtenerPorClienteAsync(clienteId));

    [HttpPost]
    public async Task<ActionResult<PedidoDto>> Crear(PedidoCreateDto dto)
    {
        var pedido = await _pedidoService.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = pedido.Id }, pedido);
    }

    [HttpPatch("{id:int}/estado")]
    public async Task<ActionResult<PedidoDto>> ActualizarEstado(int id, ActualizarEstadoPedidoDto dto) =>
        Ok(await _pedidoService.ActualizarEstadoAsync(id, dto.Estado));
}
