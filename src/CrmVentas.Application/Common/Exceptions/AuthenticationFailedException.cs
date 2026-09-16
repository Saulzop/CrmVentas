namespace CrmVentas.Application.Common.Exceptions;

public class AuthenticationFailedException : Exception
{
    public AuthenticationFailedException() : base("Usuario o contraseña incorrectos.")
    {
    }
}
