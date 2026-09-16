namespace CrmVentas.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public required string NombreUsuario { get; set; }
    public required string PasswordHash { get; set; }
    public string Rol { get; set; } = "Vendedor";
}
