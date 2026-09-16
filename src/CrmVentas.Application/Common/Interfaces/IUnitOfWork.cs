namespace CrmVentas.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task<int> GuardarCambiosAsync();
}
