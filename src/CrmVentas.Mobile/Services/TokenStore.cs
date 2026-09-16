namespace CrmVentas.Mobile.Services;

public class TokenStore
{
    private const string TokenKey = "auth_token";
    private const string UsernameKey = "auth_username";

    public string? Token { get; private set; }
    public string? NombreUsuario { get; private set; }

    public async Task CargarAsync()
    {
        Token = await SecureStorage.Default.GetAsync(TokenKey);
        NombreUsuario = await SecureStorage.Default.GetAsync(UsernameKey);
    }

    public async Task GuardarAsync(string token, string nombreUsuario)
    {
        Token = token;
        NombreUsuario = nombreUsuario;
        await SecureStorage.Default.SetAsync(TokenKey, token);
        await SecureStorage.Default.SetAsync(UsernameKey, nombreUsuario);
    }

    public void Limpiar()
    {
        Token = null;
        NombreUsuario = null;
        SecureStorage.Default.Remove(TokenKey);
        SecureStorage.Default.Remove(UsernameKey);
    }

    public bool EstaAutenticado => !string.IsNullOrEmpty(Token);
}
