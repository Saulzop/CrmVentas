using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CrmVentas.Application.Dtos;
using CrmVentas.Mobile.Services;

namespace CrmVentas.Mobile.ViewModel;

public partial class PedidosViewModel : BaseViewModel
{
    private readonly ApiClient _apiClient;

    public ObservableCollection<PedidoDto> Pedidos { get; } = [];

    public PedidosViewModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [RelayCommand]
    private async Task CargarAsync()
    {
        await EjecutarAsync(async () =>
        {
            var pedidos = await _apiClient.ObtenerPedidosAsync();
            Pedidos.Clear();
            foreach (var pedido in pedidos ?? [])
                Pedidos.Add(pedido);
        });
    }

    [RelayCommand]
    private async Task NuevoPedidoAsync() =>
        await Shell.Current.GoToAsync(nameof(Vistas.PedidoCreatePage));

    [RelayCommand]
    private async Task VerDetalleAsync(PedidoDto pedido) =>
        await Shell.Current.GoToAsync($"{nameof(Vistas.PedidoDetailPage)}?id={pedido.Id}");
}
