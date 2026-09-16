using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrmVentas.Application.Dtos;
using CrmVentas.Mobile.Services;

namespace CrmVentas.Mobile.ViewModel;

[QueryProperty(nameof(ProductoId), "id")]
public partial class ProductoDetailViewModel : BaseViewModel
{
    private readonly ApiClient _apiClient;
    private int? _productoId;

    [ObservableProperty]
    private string sku = string.Empty;

    [ObservableProperty]
    private string nombre = string.Empty;

    [ObservableProperty]
    private string? descripcion;

    [ObservableProperty]
    private string precio = "0";

    [ObservableProperty]
    private string stock = "0";

    [ObservableProperty]
    private string titulo = "Nuevo producto";

    [ObservableProperty]
    private bool esEdicion;

    public string ProductoId
    {
        set
        {
            if (int.TryParse(value, out var id))
            {
                _productoId = id;
                Titulo = "Editar producto";
                EsEdicion = true;
                _ = CargarAsync(id);
            }
        }
    }

    public ProductoDetailViewModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    private async Task CargarAsync(int id)
    {
        await EjecutarAsync(async () =>
        {
            var producto = await _apiClient.ObtenerProductoAsync(id);
            if (producto is null) return;

            Sku = producto.Sku;
            Nombre = producto.Nombre;
            Descripcion = producto.Descripcion;
            Precio = producto.Precio.ToString("0.00");
            Stock = producto.Stock.ToString();
        });
    }

    [RelayCommand]
    private async Task GuardarAsync()
    {
        await EjecutarAsync(async () =>
        {
            if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Sku))
            {
                ErrorMessage = "El SKU y el nombre son obligatorios.";
                return;
            }

            if (!decimal.TryParse(Precio, out var precioValor) || precioValor < 0)
            {
                ErrorMessage = "El precio no es válido.";
                return;
            }

            if (!int.TryParse(Stock, out var stockValor) || stockValor < 0)
            {
                ErrorMessage = "El stock no es válido.";
                return;
            }

            if (_productoId is int id)
                await _apiClient.ActualizarProductoAsync(id, new ProductoUpdateDto(Nombre, Descripcion, precioValor, stockValor));
            else
                await _apiClient.CrearProductoAsync(new ProductoCreateDto(Sku, Nombre, Descripcion, precioValor, stockValor));

            await Shell.Current.GoToAsync("..");
        });
    }
}
