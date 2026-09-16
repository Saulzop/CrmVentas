using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrmVentas.Application.Dtos;
using CrmVentas.Domain.Enums;
using CrmVentas.Mobile.Services;

namespace CrmVentas.Mobile.ViewModels;

[QueryProperty(nameof(PedidoId), "id")]
public partial class PedidoDetailViewModel : BaseViewModel
{
    private readonly ApiClient _apiClient;
    private int _pedidoId;

    private static readonly Dictionary<EstadoPedido, EstadoPedido[]> TransicionesValidas = new()
    {
        [EstadoPedido.Pendiente] = [EstadoPedido.Confirmado, EstadoPedido.Cancelado],
        [EstadoPedido.Confirmado] = [EstadoPedido.Enviado, EstadoPedido.Cancelado],
        [EstadoPedido.Enviado] = [EstadoPedido.Entregado],
        [EstadoPedido.Entregado] = [],
        [EstadoPedido.Cancelado] = []
    };

    [ObservableProperty]
    private PedidoDto? pedido;

    public ObservableCollection<EstadoPedido> SiguientesEstados { get; } = [];

    public string PedidoId
    {
        set
        {
            if (int.TryParse(value, out var id))
            {
                _pedidoId = id;
                _ = CargarAsync();
            }
        }
    }

    public PedidoDetailViewModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    private async Task CargarAsync()
    {
        await EjecutarAsync(async () =>
        {
            Pedido = await _apiClient.ObtenerPedidoAsync(_pedidoId);

            SiguientesEstados.Clear();
            if (Pedido is not null)
                foreach (var estado in TransicionesValidas[Pedido.Estado])
                    SiguientesEstados.Add(estado);
        });
    }

    [RelayCommand]
    private async Task CambiarEstadoAsync(EstadoPedido nuevoEstado)
    {
        await EjecutarAsync(async () =>
        {
            Pedido = await _apiClient.ActualizarEstadoPedidoAsync(_pedidoId, new ActualizarEstadoPedidoDto(nuevoEstado));

            SiguientesEstados.Clear();
            if (Pedido is not null)
                foreach (var estado in TransicionesValidas[Pedido.Estado])
                    SiguientesEstados.Add(estado);
        });
    }
}
