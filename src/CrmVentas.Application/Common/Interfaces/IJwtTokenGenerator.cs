using CrmVentas.Domain.Entities;

namespace CrmVentas.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerarToken(Usuario usuario);
}
