using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CrmVentas.Application.Dtos;
using CrmVentas.Mobile.Services;

namespace CrmVentas.Mobile.ViewModel;

public partial class ProductosViewModel : BaseViewModel
{
    private readonly ApiClient _apiClient;

    public ObservableCollection<ProductoDto> Productos { get; } = [];

    public ProductosViewModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [RelayCommand]
    private async Task CargarAsync()
    {
        await EjecutarAsync(async () =>
        {
            var productos = await _apiClient.ObtenerProductosAsync();
            Productos.Clear();
            foreach (var producto in productos ?? [])
                Productos.Add(producto);
        });
    }

    [RelayCommand]
    private async Task AgregarAsync() =>
        await Shell.Current.GoToAsync(nameof(Vistas.ProductoDetailPage));

    [RelayCommand]
    private async Task EditarAsync(ProductoDto producto) =>
        await Shell.Current.GoToAsync($"{nameof(Vistas.ProductoDetailPage)}?id={producto.Id}");

    [RelayCommand]
    private async Task EliminarAsync(ProductoDto producto)
    {
        await EjecutarAsync(async () =>
        {
            await _apiClient.EliminarProductoAsync(producto.Id);
            Productos.Remove(producto);
        });
    }
}
