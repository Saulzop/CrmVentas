using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrmVentas.Application.Dtos;
using CrmVentas.Mobile.Services;

namespace CrmVentas.Mobile.ViewModel;

public partial class PedidoCreateViewModel : BaseViewModel
{
    private readonly ApiClient _apiClient;

    public ObservableCollection<ClienteDto> Clientes { get; } = [];
    public ObservableCollection<ProductoDto> Productos { get; } = [];
    public ObservableCollection<ItemPedidoEnConstruccion> Items { get; } = [];

    [ObservableProperty]
    private ClienteDto? clienteSeleccionado;

    public PedidoCreateViewModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
        _ = CargarCatalogosAsync();
    }

    private async Task CargarCatalogosAsync()
    {
        await EjecutarAsync(async () =>
        {
            var clientes = await _apiClient.ObtenerClientesAsync();
            Clientes.Clear();
            foreach (var cliente in clientes ?? [])
                Clientes.Add(cliente);

            var productos = await _apiClient.ObtenerProductosAsync();
            Productos.Clear();
            foreach (var producto in productos ?? [])
                Productos.Add(producto);

            if (Items.Count == 0)
                Items.Add(new ItemPedidoEnConstruccion());
        });
    }

    [RelayCommand]
    private void AgregarItem() => Items.Add(new ItemPedidoEnConstruccion());

    [RelayCommand]
    private void QuitarItem(ItemPedidoEnConstruccion item)
    {
        if (Items.Count > 1)
            Items.Remove(item);
    }

    [RelayCommand]
    private async Task GuardarAsync()
    {
        await EjecutarAsync(async () =>
        {
            if (ClienteSeleccionado is null)
            {
                ErrorMessage = "Selecciona un cliente.";
                return;
            }

            var itemsDto = new List<PedidoItemCreateDto>();
            foreach (var item in Items)
            {
                if (item.ProductoSeleccionado is null) continue;
                if (!int.TryParse(item.Cantidad, out var cantidad) || cantidad <= 0)
                {
                    ErrorMessage = $"Cantidad inválida para '{item.ProductoSeleccionado.Nombre}'.";
                    return;
                }
                itemsDto.Add(new PedidoItemCreateDto(item.ProductoSeleccionado.Id, cantidad));
            }

            if (itemsDto.Count == 0)
            {
                ErrorMessage = "Agrega al menos un producto al pedido.";
                return;
            }

            await _apiClient.CrearPedidoAsync(new PedidoCreateDto(ClienteSeleccionado.Id, itemsDto));
            await Shell.Current.GoToAsync("..");
        });
    }
}
