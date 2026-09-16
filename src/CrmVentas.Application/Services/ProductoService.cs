using CrmVentas.Application.Common.Exceptions;
using CrmVentas.Application.Common.Interfaces;
using CrmVentas.Application.Dtos;
using CrmVentas.Application.Services.Interfaces;
using CrmVentas.Domain.Entities;

namespace CrmVentas.Application.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductoService(IProductoRepository productoRepository, IUnitOfWork unitOfWork)
    {
        _productoRepository = productoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ProductoDto>> ObtenerTodosAsync()
    {
        var productos = await _productoRepository.ObtenerTodosAsync();
        return productos.Select(MapearADto).ToList();
    }

    public async Task<ProductoDto> ObtenerPorIdAsync(int id)
    {
        var producto = await _productoRepository.ObtenerPorIdAsync(id)
            ?? throw new NotFoundException(nameof(Producto), id);
        return MapearADto(producto);
    }

    public async Task<ProductoDto> CrearAsync(ProductoCreateDto dto)
    {
        if (await _productoRepository.ExisteSkuAsync(dto.Sku))
            throw new BusinessRuleException($"Ya existe un producto con el SKU '{dto.Sku}'.");

        var producto = new Producto
        {
            Sku = dto.Sku,
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Precio = dto.Precio,
            Stock = dto.Stock
        };

        await _productoRepository.AgregarAsync(producto);
        await _unitOfWork.GuardarCambiosAsync();

        return MapearADto(producto);
    }

    public async Task<ProductoDto> ActualizarAsync(int id, ProductoUpdateDto dto)
    {
        var producto = await _productoRepository.ObtenerPorIdAsync(id)
            ?? throw new NotFoundException(nameof(Producto), id);

        producto.Nombre = dto.Nombre;
        producto.Descripcion = dto.Descripcion;
        producto.Precio = dto.Precio;
        producto.Stock = dto.Stock;

        _productoRepository.Actualizar(producto);
        await _unitOfWork.GuardarCambiosAsync();

        return MapearADto(producto);
    }

    public async Task EliminarAsync(int id)
    {
        var producto = await _productoRepository.ObtenerPorIdAsync(id)
            ?? throw new NotFoundException(nameof(Producto), id);

        _productoRepository.Eliminar(producto);
        await _unitOfWork.GuardarCambiosAsync();
    }

    private static ProductoDto MapearADto(Producto producto) => new(
        producto.Id,
        producto.Sku,
        producto.Nombre,
        producto.Descripcion,
        producto.Precio,
        producto.Stock);
}
