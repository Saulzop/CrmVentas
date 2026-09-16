using CrmVentas.Domain.Modelos;

namespace CrmVentas.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerarToken(Usuario usuario);
}
