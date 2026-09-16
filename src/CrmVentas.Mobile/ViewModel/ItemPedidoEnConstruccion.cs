using CommunityToolkit.Mvvm.ComponentModel;
using CrmVentas.Application.Dtos;

namespace CrmVentas.Mobile.ViewModel;

public partial class ItemPedidoEnConstruccion : ObservableObject
{
    [ObservableProperty]
    private ProductoDto? productoSeleccionado;

    [ObservableProperty]
    private string cantidad = "1";
}
