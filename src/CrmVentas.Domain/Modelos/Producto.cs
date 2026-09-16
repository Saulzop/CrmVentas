namespace CrmVentas.Domain.Modelos;

public class Producto
{
    public int Id { get; set; }
    public required string Sku { get; set; }
    public required string Nombre { get; set; }
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }

    public ICollection<PedidoItem> PedidoItems { get; set; } = new List<PedidoItem>();
}
