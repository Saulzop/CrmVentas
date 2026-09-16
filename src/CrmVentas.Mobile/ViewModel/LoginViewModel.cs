using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrmVentas.Application.Dtos;
using CrmVentas.Mobile.Services;

namespace CrmVentas.Mobile.ViewModel;

public partial class LoginViewModel : BaseViewModel
{
    private readonly ApiClient _apiClient;
    private readonly TokenStore _tokenStore;

    [ObservableProperty]
    private string nombreUsuario = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    public LoginViewModel(ApiClient apiClient, TokenStore tokenStore)
    {
        _apiClient = apiClient;
        _tokenStore = tokenStore;
    }

    [RelayCommand]
    private async Task IniciarSesionAsync()
    {
        await EjecutarAsync(async () =>
        {
            if (string.IsNullOrWhiteSpace(NombreUsuario) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Ingresa tu usuario y contraseña.";
                return;
            }

            var respuesta = await _apiClient.LoginAsync(new LoginRequestDto(NombreUsuario, Password));
            if (respuesta is null)
            {
                ErrorMessage = "No se pudo iniciar sesión.";
                return;
            }

            await _tokenStore.GuardarAsync(respuesta.Token, respuesta.NombreUsuario);
            await Shell.Current.GoToAsync("//main/clientes");
        });
    }
}
