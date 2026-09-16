using CrmVentas.Application.Dtos;
using CrmVentas.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmVentas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ClienteDto>>> ObtenerTodos() =>
        Ok(await _clienteService.ObtenerTodosAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteDto>> ObtenerPorId(int id) =>
        Ok(await _clienteService.ObtenerPorIdAsync(id));

    [HttpPost]
    public async Task<ActionResult<ClienteDto>> Crear(ClienteCreateDto dto)
    {
        var cliente = await _clienteService.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = cliente.Id }, cliente);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ClienteDto>> Actualizar(int id, ClienteUpdateDto dto) =>
        Ok(await _clienteService.ActualizarAsync(id, dto));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _clienteService.EliminarAsync(id);
        return NoContent();
    }
}
