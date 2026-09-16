using CrmVentas.Application.Dtos;

namespace CrmVentas.Application.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
}
