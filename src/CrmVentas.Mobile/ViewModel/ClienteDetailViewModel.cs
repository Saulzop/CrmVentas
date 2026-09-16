using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrmVentas.Application.Dtos;
using CrmVentas.Mobile.Services;

namespace CrmVentas.Mobile.ViewModel;

[QueryProperty(nameof(ClienteId), "id")]
public partial class ClienteDetailViewModel : BaseViewModel
{
    private readonly ApiClient _apiClient;
    private int? _clienteId;

    [ObservableProperty]
    private string nombre = string.Empty;

    [ObservableProperty]
    private string? email;

    [ObservableProperty]
    private string? telefono;

    [ObservableProperty]
    private string? direccion;

    [ObservableProperty]
    private string titulo = "Nuevo cliente";

    public string ClienteId
    {
        set
        {
            if (int.TryParse(value, out var id))
            {
                _clienteId = id;
                Titulo = "Editar cliente";
                _ = CargarAsync(id);
            }
        }
    }

    public ClienteDetailViewModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    private async Task CargarAsync(int id)
    {
        await EjecutarAsync(async () =>
        {
            var cliente = await _apiClient.ObtenerClienteAsync(id);
            if (cliente is null) return;

            Nombre = cliente.Nombre;
            Email = cliente.Email;
            Telefono = cliente.Telefono;
            Direccion = cliente.Direccion;
        });
    }

    [RelayCommand]
    private async Task GuardarAsync()
    {
        await EjecutarAsync(async () =>
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                ErrorMessage = "El nombre es obligatorio.";
                return;
            }

            if (_clienteId is int id)
                await _apiClient.ActualizarClienteAsync(id, new ClienteUpdateDto(Nombre, Email, Telefono, Direccion));
            else
                await _apiClient.CrearClienteAsync(new ClienteCreateDto(Nombre, Email, Telefono, Direccion));

            await Shell.Current.GoToAsync("..");
        });
    }
}
