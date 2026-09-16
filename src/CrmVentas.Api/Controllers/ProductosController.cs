using CrmVentas.Application.Dtos;
using CrmVentas.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrmVentas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductoDto>>> ObtenerTodos() =>
        Ok(await _productoService.ObtenerTodosAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductoDto>> ObtenerPorId(int id) =>
        Ok(await _productoService.ObtenerPorIdAsync(id));

    [HttpPost]
    public async Task<ActionResult<ProductoDto>> Crear(ProductoCreateDto dto)
    {
        var producto = await _productoService.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = producto.Id }, producto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductoDto>> Actualizar(int id, ProductoUpdateDto dto) =>
        Ok(await _productoService.ActualizarAsync(id, dto));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _productoService.EliminarAsync(id);
        return NoContent();
    }
}
