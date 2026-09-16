using CrmVentas.Mobile.ViewModels;

namespace CrmVentas.Mobile.Views;

public partial class ClienteDetailPage : ContentPage
{
    public ClienteDetailPage(ClienteDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
