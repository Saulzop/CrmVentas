using CrmVentas.Mobile.ViewModels;

namespace CrmVentas.Mobile.Views;

public partial class ProductoDetailPage : ContentPage
{
    public ProductoDetailPage(ProductoDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
