using CrmVentas.Application.Common.Exceptions;
using CrmVentas.Application.Common.Interfaces;
using CrmVentas.Application.Dtos;
using CrmVentas.Application.Services.Interfaces;
using CrmVentas.Domain.Modelos;

namespace CrmVentas.Application.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ClienteService(IClienteRepository clienteRepository, IUnitOfWork unitOfWork)
    {
        _clienteRepository = clienteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ClienteDto>> ObtenerTodosAsync()
    {
        var clientes = await _clienteRepository.ObtenerTodosAsync();
        return clientes.Select(MapearADto).ToList();
    }

    public async Task<ClienteDto> ObtenerPorIdAsync(int id)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(id)
            ?? throw new NotFoundException(nameof(Cliente), id);
        return MapearADto(cliente);
    }

    public async Task<ClienteDto> CrearAsync(ClienteCreateDto dto)
    {
        var cliente = new Cliente
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            Telefono = dto.Telefono,
            Direccion = dto.Direccion
        };

        await _clienteRepository.AgregarAsync(cliente);
        await _unitOfWork.GuardarCambiosAsync();

        return MapearADto(cliente);
    }

    public async Task<ClienteDto> ActualizarAsync(int id, ClienteUpdateDto dto)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(id)
            ?? throw new NotFoundException(nameof(Cliente), id);

        cliente.Nombre = dto.Nombre;
        cliente.Email = dto.Email;
        cliente.Telefono = dto.Telefono;
        cliente.Direccion = dto.Direccion;

        _clienteRepository.Actualizar(cliente);
        await _unitOfWork.GuardarCambiosAsync();

        return MapearADto(cliente);
    }

    public async Task EliminarAsync(int id)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(id)
            ?? throw new NotFoundException(nameof(Cliente), id);

        _clienteRepository.Eliminar(cliente);
        await _unitOfWork.GuardarCambiosAsync();
    }

    private static ClienteDto MapearADto(Cliente cliente) => new(
        cliente.Id,
        cliente.Nombre,
        cliente.Email,
        cliente.Telefono,
        cliente.Direccion,
        cliente.FechaRegistro);
}
