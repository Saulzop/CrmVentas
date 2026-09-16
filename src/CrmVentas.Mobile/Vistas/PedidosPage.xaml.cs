using CrmVentas.Mobile.ViewModel;

namespace CrmVentas.Mobile.Vistas;

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
