using CommunityToolkit.Mvvm.ComponentModel;

namespace CrmVentas.Mobile.ViewModel;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    protected async Task EjecutarAsync(Func<Task> accion)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;
            await accion();
        }
        catch (Services.ApiException ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
