using System.Net.Http.Json;
using System.Text.Json;
using CrmVentas.Application.Dtos;

namespace CrmVentas.Mobile.Services;

public class ApiClient
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public ApiClient(HttpClient http)
    {
        _http = http;
    }

    public Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto) =>
        EnviarAsync<LoginResponseDto>(() => _http.PostAsJsonAsync("api/auth/login", dto));

    public Task<List<ClienteDto>?> ObtenerClientesAsync() =>
        EnviarAsync<List<ClienteDto>>(() => _http.GetAsync("api/clientes"));

    public Task<ClienteDto?> ObtenerClienteAsync(int id) =>
        EnviarAsync<ClienteDto>(() => _http.GetAsync($"api/clientes/{id}"));

    public Task<ClienteDto?> CrearClienteAsync(ClienteCreateDto dto) =>
        EnviarAsync<ClienteDto>(() => _http.PostAsJsonAsync("api/clientes", dto));

    public Task<ClienteDto?> ActualizarClienteAsync(int id, ClienteUpdateDto dto) =>
        EnviarAsync<ClienteDto>(() => _http.PutAsJsonAsync($"api/clientes/{id}", dto));

    public Task EliminarClienteAsync(int id) =>
        EnviarSinRespuestaAsync(() => _http.DeleteAsync($"api/clientes/{id}"));

    public Task<List<ProductoDto>?> ObtenerProductosAsync() =>
        EnviarAsync<List<ProductoDto>>(() => _http.GetAsync("api/productos"));

    public Task<ProductoDto?> ObtenerProductoAsync(int id) =>
        EnviarAsync<ProductoDto>(() => _http.GetAsync($"api/productos/{id}"));

    public Task<ProductoDto?> CrearProductoAsync(ProductoCreateDto dto) =>
        EnviarAsync<ProductoDto>(() => _http.PostAsJsonAsync("api/productos", dto));

    public Task<ProductoDto?> ActualizarProductoAsync(int id, ProductoUpdateDto dto) =>
        EnviarAsync<ProductoDto>(() => _http.PutAsJsonAsync($"api/productos/{id}", dto));

    public Task EliminarProductoAsync(int id) =>
        EnviarSinRespuestaAsync(() => _http.DeleteAsync($"api/productos/{id}"));

    public Task<List<PedidoDto>?> ObtenerPedidosAsync() =>
        EnviarAsync<List<PedidoDto>>(() => _http.GetAsync("api/pedidos"));

    public Task<PedidoDto?> ObtenerPedidoAsync(int id) =>
        EnviarAsync<PedidoDto>(() => _http.GetAsync($"api/pedidos/{id}"));

    public Task<PedidoDto?> CrearPedidoAsync(PedidoCreateDto dto) =>
        EnviarAsync<PedidoDto>(() => _http.PostAsJsonAsync("api/pedidos", dto));

    public Task<PedidoDto?> ActualizarEstadoPedidoAsync(int id, ActualizarEstadoPedidoDto dto) =>
        EnviarAsync<PedidoDto>(() => _http.PatchAsJsonAsync($"api/pedidos/{id}/estado", dto));

    private static async Task<T?> EnviarAsync<T>(Func<Task<HttpResponseMessage>> enviar)
    {
        HttpResponseMessage response;
        try
        {
            response = await enviar();
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            throw new ApiException("No se pudo conectar con el servidor. Verifica tu conexión.");
        }

        if (!response.IsSuccessStatusCode)
            throw new ApiException(await LeerMensajeErrorAsync(response), (int)response.StatusCode);

        return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
    }

    private static async Task EnviarSinRespuestaAsync(Func<Task<HttpResponseMessage>> enviar)
    {
        HttpResponseMessage response;
        try
        {
            response = await enviar();
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            throw new ApiException("No se pudo conectar con el servidor. Verifica tu conexión.");
        }

        if (!response.IsSuccessStatusCode)
            throw new ApiException(await LeerMensajeErrorAsync(response), (int)response.StatusCode);
    }

    private static async Task<string> LeerMensajeErrorAsync(HttpResponseMessage response)
    {
        try
        {
            var body = await response.Content.ReadFromJsonAsync<JsonElement>();
            if (body.TryGetProperty("error", out var error))
                return error.GetString() ?? "Ocurrió un error inesperado.";
        }
        catch
        {
            // El cuerpo no tenía el formato { error: "..." } esperado; se usa el mensaje genérico.
        }

        return response.StatusCode == System.Net.HttpStatusCode.Unauthorized
            ? "Tu sesión expiró. Inicia sesión de nuevo."
            : "Ocurrió un error inesperado.";
    }
}
