using CrmVentas.Mobile.ViewModels;

namespace CrmVentas.Mobile.Views;

public partial class PedidoCreatePage : ContentPage
{
    public PedidoCreatePage(PedidoCreateViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
