using CrmVentas.Mobile.ViewModels;

namespace CrmVentas.Mobile.Views;

public partial class PedidoDetailPage : ContentPage
{
    public PedidoDetailPage(PedidoDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
