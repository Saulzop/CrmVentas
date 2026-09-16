using CrmVentas.Mobile.Vistas;

namespace CrmVentas.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(ClienteDetailPage), typeof(ClienteDetailPage));
        Routing.RegisterRoute(nameof(ProductoDetailPage), typeof(ProductoDetailPage));
        Routing.RegisterRoute(nameof(PedidoCreatePage), typeof(PedidoCreatePage));
        Routing.RegisterRoute(nameof(PedidoDetailPage), typeof(PedidoDetailPage));
    }
}
