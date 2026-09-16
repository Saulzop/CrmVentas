using CrmVentas.Application.Services;
using CrmVentas.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CrmVentas.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IProductoService, ProductoService>();
        services.AddScoped<IPedidoService, PedidoService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
