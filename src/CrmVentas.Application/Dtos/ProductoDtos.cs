namespace CrmVentas.Application.Dtos;

public record ProductoDto(
    int Id,
    string Sku,
    string Nombre,
    string? Descripcion,
    decimal Precio,
    int Stock);

public record ProductoCreateDto(
    string Sku,
    string Nombre,
    string? Descripcion,
    decimal Precio,
    int Stock);

public record ProductoUpdateDto(
    string Nombre,
    string? Descripcion,
    decimal Precio,
    int Stock);
