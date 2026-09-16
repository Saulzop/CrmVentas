using CrmVentas.Mobile.ViewModel;

namespace CrmVentas.Mobile.Vistas;

public partial class PedidoDetailPage : ContentPage
{
    public PedidoDetailPage(PedidoDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
