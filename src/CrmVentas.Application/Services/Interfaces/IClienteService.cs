using CrmVentas.Application.Dtos;

namespace CrmVentas.Application.Services.Interfaces;

public interface IClienteService
{
    Task<List<ClienteDto>> ObtenerTodosAsync();
    Task<ClienteDto> ObtenerPorIdAsync(int id);
    Task<ClienteDto> CrearAsync(ClienteCreateDto dto);
    Task<ClienteDto> ActualizarAsync(int id, ClienteUpdateDto dto);
    Task EliminarAsync(int id);
}
