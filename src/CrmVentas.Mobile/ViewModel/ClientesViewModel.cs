using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CrmVentas.Application.Dtos;
using CrmVentas.Mobile.Services;

namespace CrmVentas.Mobile.ViewModel;

public partial class ClientesViewModel : BaseViewModel
{
    private readonly ApiClient _apiClient;

    public ObservableCollection<ClienteDto> Clientes { get; } = [];

    public ClientesViewModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [RelayCommand]
    private async Task CargarAsync()
    {
        await EjecutarAsync(async () =>
        {
            var clientes = await _apiClient.ObtenerClientesAsync();
            Clientes.Clear();
            foreach (var cliente in clientes ?? [])
                Clientes.Add(cliente);
        });
    }

    [RelayCommand]
    private async Task AgregarAsync() =>
        await Shell.Current.GoToAsync(nameof(Vistas.ClienteDetailPage));

    [RelayCommand]
    private async Task EditarAsync(ClienteDto cliente) =>
        await Shell.Current.GoToAsync($"{nameof(Vistas.ClienteDetailPage)}?id={cliente.Id}");

    [RelayCommand]
    private async Task EliminarAsync(ClienteDto cliente)
    {
        await EjecutarAsync(async () =>
        {
            await _apiClient.EliminarClienteAsync(cliente.Id);
            Clientes.Remove(cliente);
        });
    }
}
