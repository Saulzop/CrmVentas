namespace CrmVentas.Application.Dtos;

public record ClienteDto(
    int Id,
    string Nombre,
    string? Email,
    string? Telefono,
    string? Direccion,
    DateTime FechaRegistro);

public record ClienteCreateDto(
    string Nombre,
    string? Email,
    string? Telefono,
    string? Direccion);

public record ClienteUpdateDto(
    string Nombre,
    string? Email,
    string? Telefono,
    string? Direccion);
