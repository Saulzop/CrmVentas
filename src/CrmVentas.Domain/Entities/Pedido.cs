using CrmVentas.Domain.Enums;

namespace CrmVentas.Domain.Entities;

public class Pedido
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public DateTime FechaPedido { get; set; } = DateTime.UtcNow;
    public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;

    public ICollection<PedidoItem> Items { get; set; } = new List<PedidoItem>();

    public decimal Total => Items.Sum(i => i.Cantidad * i.PrecioUnitario);
}
