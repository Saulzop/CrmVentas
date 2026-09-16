namespace CrmVentas.Mobile.Services;

public static class AppConfig
{
    // El emulador de Android no puede usar "localhost" para llegar al equipo anfitrión;
    // 10.0.2.2 es la IP especial que el emulador mapea a la máquina host.
    public static string ApiBaseUrl =>
#if ANDROID
        "http://10.0.2.2:5119/";
#else
        "http://localhost:5119/";
#endif
}
