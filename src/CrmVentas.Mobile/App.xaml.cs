using CrmVentas.Mobile.Services;

namespace CrmVentas.Mobile;

public partial class App : Microsoft.Maui.Controls.Application
{
    private readonly TokenStore _tokenStore;

    public App(TokenStore tokenStore)
    {
        InitializeComponent();
        _tokenStore = tokenStore;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());
        window.Created += async (_, _) => await IrAPantallaInicialAsync();
        return window;
    }

    private async Task IrAPantallaInicialAsync()
    {
        await _tokenStore.CargarAsync();
        if (_tokenStore.EstaAutenticado)
            await Shell.Current.GoToAsync("//main/clientes");
    }
}
