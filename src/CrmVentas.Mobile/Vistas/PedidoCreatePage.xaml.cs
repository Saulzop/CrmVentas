using CrmVentas.Mobile.ViewModel;

namespace CrmVentas.Mobile.Vistas;

public partial class PedidoCreatePage : ContentPage
{
    public PedidoCreatePage(PedidoCreateViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
