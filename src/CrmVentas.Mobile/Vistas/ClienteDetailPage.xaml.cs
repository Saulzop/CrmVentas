using CrmVentas.Mobile.ViewModel;

namespace CrmVentas.Mobile.Vistas;

public partial class ClienteDetailPage : ContentPage
{
    public ClienteDetailPage(ClienteDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
