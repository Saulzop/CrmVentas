using CrmVentas.Mobile.ViewModel;

namespace CrmVentas.Mobile.Vistas;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
