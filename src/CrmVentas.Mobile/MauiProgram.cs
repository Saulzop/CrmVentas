using CrmVentas.Mobile.Services;
using CrmVentas.Mobile.ViewModel;
using CrmVentas.Mobile.Vistas;
using Microsoft.Extensions.Logging;

namespace CrmVentas.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<TokenStore>();
        builder.Services.AddTransient<AuthHeaderHandler>();

        builder.Services.AddHttpClient<ApiClient>(client =>
        {
            client.BaseAddress = new Uri(AppConfig.ApiBaseUrl);
        }).AddHttpMessageHandler<AuthHeaderHandler>();

        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();

        builder.Services.AddTransient<ClientesViewModel>();
        builder.Services.AddTransient<ClientesPage>();
        builder.Services.AddTransient<ClienteDetailViewModel>();
        builder.Services.AddTransient<ClienteDetailPage>();

        builder.Services.AddTransient<ProductosViewModel>();
        builder.Services.AddTransient<ProductosPage>();
        builder.Services.AddTransient<ProductoDetailViewModel>();
        builder.Services.AddTransient<ProductoDetailPage>();

        builder.Services.AddTransient<PedidosViewModel>();
        builder.Services.AddTransient<PedidosPage>();
        builder.Services.AddTransient<PedidoCreateViewModel>();
        builder.Services.AddTransient<PedidoCreatePage>();
        builder.Services.AddTransient<PedidoDetailViewModel>();
        builder.Services.AddTransient<PedidoDetailPage>();

        return builder.Build();
    }
}
