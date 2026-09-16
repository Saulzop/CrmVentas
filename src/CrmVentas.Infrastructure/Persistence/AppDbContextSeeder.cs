using CrmVentas.Application.Common.Interfaces;
using CrmVentas.Domain.Modelos;

namespace CrmVentas.Infrastructure.Persistence;

public static class AppDbContextSeeder
{
    public static async Task SeedAsync(AppDbContext context, IPasswordHasher passwordHasher)
    {
        if (!context.Usuarios.Any())
        {
            context.Usuarios.Add(new Usuario
            {
                NombreUsuario = "admin",
                PasswordHash = passwordHasher.Hash("Admin123!"),
                Rol = "Administrador"
            });
        }

        if (!context.Clientes.Any())
        {
            context.Clientes.AddRange(
                new Cliente { Nombre = "Comercializadora del Valle S.A. de C.V.", Email = "contacto@delvalle.mx", Telefono = "55 1234 5678", Direccion = "Ecatepec, Estado de México" },
                new Cliente { Nombre = "Distribuidora Peninsular", Email = "ventas@peninsular.mx", Telefono = "999 876 5432", Direccion = "Mérida, Yucatán" },
                new Cliente { Nombre = "Grupo Industrial Chiapas", Email = "info@grupochiapas.mx", Telefono = "961 222 3344", Direccion = "Tuxtla Gutiérrez, Chiapas" }
            );
        }

        if (!context.Productos.Any())
        {
            context.Productos.AddRange(
                new Producto { Sku = "PRD-001", Nombre = "Tarima industrial 40x48", Descripcion = "Tarima de plástico reforzado", Precio = 350.00m, Stock = 120 },
                new Producto { Sku = "PRD-002", Nombre = "Contenedor plegable 600L", Descripcion = "Contenedor plástico apilable", Precio = 890.50m, Stock = 45 },
                new Producto { Sku = "PRD-003", Nombre = "Rollo film estirable 20\"", Descripcion = "Film para embalaje de pallets", Precio = 120.00m, Stock = 300 }
            );
        }

        await context.SaveChangesAsync();
    }
}
