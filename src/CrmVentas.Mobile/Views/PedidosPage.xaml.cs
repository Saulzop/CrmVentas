using CrmVentas.Mobile.ViewModels;

namespace CrmVentas.Mobile.Views;

public partial class PedidosPage : ContentPage
{
    private readonly PedidosViewModel _viewModel;

    public PedidosPage(PedidosViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.CargarCommand.Execute(null);
    }
}
