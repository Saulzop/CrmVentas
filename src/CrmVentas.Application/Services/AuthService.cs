using CrmVentas.Application.Common.Exceptions;
using CrmVentas.Application.Common.Interfaces;
using CrmVentas.Application.Dtos;
using CrmVentas.Application.Services.Interfaces;

namespace CrmVentas.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var usuario = await _usuarioRepository.ObtenerPorNombreUsuarioAsync(dto.NombreUsuario)
            ?? throw new AuthenticationFailedException();

        if (!_passwordHasher.Verify(dto.Password, usuario.PasswordHash))
            throw new AuthenticationFailedException();

        var token = _jwtTokenGenerator.GenerarToken(usuario);

        return new LoginResponseDto(token, usuario.NombreUsuario, usuario.Rol, DateTime.UtcNow.AddHours(8));
    }
}
