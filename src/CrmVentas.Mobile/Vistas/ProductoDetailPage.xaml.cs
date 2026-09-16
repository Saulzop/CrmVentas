using CrmVentas.Mobile.ViewModel;

namespace CrmVentas.Mobile.Vistas;

public partial class ProductoDetailPage : ContentPage
{
    public ProductoDetailPage(ProductoDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
